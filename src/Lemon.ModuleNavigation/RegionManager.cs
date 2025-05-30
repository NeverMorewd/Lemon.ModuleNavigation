using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Lemon.ModuleNavigation.Internal;
using System.Collections.Concurrent;
namespace Lemon.ModuleNavigation;

public class RegionManager : IRegionManager
{
    private readonly ConcurrentDictionary<string, IRegion> _regions = [];
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, ConcurrentStack<NavigationContext>> _buffer = [];
    private readonly ConcurrentSet<IObserver<NavigationContext>> _navigationObservers = [];
    private readonly ConcurrentSet<IObserver<IRegion>> _regionsObservers = [];
    public RegionManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [Obsolete("requestNew was obsolete.Consider IsNavigationTarget() in INavigationAware instead.")]
    public void RequestNavigate(string regionName, string viewName, bool requestNew, NavigationParameters? parameters = null)
    {
        var context = new NavigationContext(viewName, regionName, _serviceProvider, requestNew, parameters);
        if (_regions.TryGetValue(regionName, out var region))
        {
            region.Activate(context);
            ToNavigationObservers(context);
        }
        else
        {
            _buffer.AddOrUpdate(regionName,
                key => 
                {
                    var stack = new ConcurrentStack<NavigationContext>();
                    stack.Push(context);
                    return stack;
                }, 
                (key, value) =>
                {
                    value.Push(context);
                    return value;
                });
        }
    }
    public void RequestViewNavigate(string regionName, string viewName, NavigationParameters? parameters = null, string? alias = null)
    {
        var context = new NavigationContext(viewName, regionName, _serviceProvider, parameters, alias);
        if (_regions.TryGetValue(regionName, out var region))
        {
            region.Activate(context);
            ToNavigationObservers(context);
        }
        else
        {
            _buffer.AddOrUpdate(regionName,
                key =>
                {
                    var stack = new ConcurrentStack<NavigationContext>();
                    stack.Push(context);
                    return stack;
                },
                (key, value) =>
                {
                    value.Push(context);
                    return value;
                });
        }
    }

    public void AddRegion(string regionName, IRegion region)
    {
        if (_regions.TryAdd(regionName, region))
        {
            ToRegionsObservers(region);

            if (_buffer.TryGetValue(regionName, out var navigationContexts))
            {
                if (navigationContexts.TryPop(out var context))
                {
                    region.Activate(context);
                    ToNavigationObservers(context);
                }
            }
        }
        else
        {
            throw new InvalidOperationException($"Duplicate key {regionName}");
        }
    }

    public IRegion? GetRegion(string regionName)
    {
        _regions.TryGetValue(regionName, out var region);
        return region;
    }

    public void RequestViewUnload(string regionName, string viewName)
    {
        if (_regions.TryGetValue(regionName, out var region))
        {
             region.DeActivate(viewName);
        }
        else
        {
            throw new RegionNameNotFoundException(nameof(regionName));
        }
    }
    public void RequestViewUnload(NavigationContext navigationContext)
    {
        if (_regions.TryGetValue(navigationContext.RegionName, out var region))
        {
            region.DeActivate(navigationContext);
        }
        else
        {
            throw new RegionNameNotFoundException(nameof(navigationContext.RegionName));
        }
    }

    public IDisposable Subscribe(IObserver<NavigationContext> observer)
    {
        if (!_navigationObservers.Add(observer))
        {
            throw new InvalidOperationException("Duplicate subscription is not allowed!");
        }
        return new DisposableAction(() =>
        {
            _navigationObservers.Remove(observer);
        });
    }
    public IDisposable Subscribe(IObserver<IRegion> observer)
    {
        if (!_regionsObservers.Add(observer))
        {
            throw new InvalidOperationException("Duplicate subscription is not allowed!");
        }
        return new DisposableAction(() =>
        {
            _regionsObservers.Remove(observer);
        });
    }
    private void ToNavigationObservers(NavigationContext navigationContext)
    {
        if (_navigationObservers.Count > 0)
        {
            Task.Run(() =>
            {
                foreach (var observer in _navigationObservers)
                {
                    observer.OnNext(navigationContext);
                }
            });
        }
    }
    private void ToRegionsObservers(IRegion region)
    {
        if (_regionsObservers.Count > 0)
        {
            Task.Run(() =>
            {
                foreach (var observer in _regionsObservers)
                {
                    observer.OnNext(region);
                }
            });
        }
    }

    public void RequestModuleNavigate(string regionName, string moduleName, NavigationParameters? parameters)
    {
        throw new NotImplementedException();
    }

    public void RequestModuleNavigate(string regionName, IModule module, NavigationParameters? parameters)
    {
        throw new NotImplementedException();
    }

    public void RequestModuleUnload(string moduleName, string viewName)
    {
        throw new NotImplementedException();
    }

    public void RequestModuleUnload(IModule module)
    {
        throw new NotImplementedException();
    }
}
