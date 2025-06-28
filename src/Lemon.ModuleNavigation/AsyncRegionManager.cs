using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation;

public class AsyncRegionManager : IAsyncRegionManager
{
    private readonly ConcurrentDictionary<string, IAsyncRegion> _regions = [];
    private readonly ConcurrentDictionary<string, ConcurrentStack<NavigationContext>> _buffer = [];
    private readonly IServiceProvider _serviceProvider;
    public AsyncRegionManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public void AddRegion(string regionName, IAsyncRegion region)
    {
        if (_regions.TryAdd(regionName, region))
        {
            //ToRegionsObservers(region);

            if (_buffer.TryGetValue(regionName, out var navigationContexts))
            {
                if (navigationContexts.TryPop(out var context))
                {
                    _ = region.ActivateAsync(context);
                    //ToNavigationObservers(context);
                }
            }
        }
        else
        {
            throw new InvalidOperationException($"Duplicate key {regionName}");
        }
    }

    public IAsyncRegion? GetRegion(string regionName)
    {
        if (_regions.TryGetValue(regionName, out var region))
        {
            return region;
        }
        return null;
    }

    public async Task RequestViewNavigateAsync(string regionName, string viewName, NavigationParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var context = new NavigationContext(viewName, regionName, _serviceProvider, parameters);
        if (_regions.TryGetValue(regionName, out var region))
        {
            await region.ActivateAsync(context);
            //ToNavigationObservers(context);
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

    public async Task RequestViewUnloadAsync(string regionName, string viewName, CancellationToken cancellationToken = default)
    {
        if (_regions.TryGetValue(regionName, out var region))
        {
            await region.DeActivateAsync(viewName);
        }
        else
        {
            throw new RegionNameNotFoundException(nameof(regionName));
        }
    }

    public async Task RequestViewUnloadAsync(NavigationContext context, CancellationToken cancellationToken = default)
    {
        if (_regions.TryGetValue(context.RegionName, out var region))
        {
            await region.DeActivateAsync(context);
        }
        else
        {
            throw new RegionNameNotFoundException(nameof(context.RegionName));
        }
    }
}
