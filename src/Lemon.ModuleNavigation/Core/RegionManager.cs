using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Internals;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Core
{

    public class RegionManager : IRegionManager
    {
        private readonly ConcurrentDictionary<string, IRegion> _regions = [];
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentStack<NavigationContext> _buffer = [];
        private readonly ConcurrentSet<IObserver<NavigationContext>> _navigationObservers = new();
        private readonly ConcurrentSet<IObserver<IRegion>> _regionsObservers = new();
        public RegionManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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
                _buffer.Push(context);
            }
        }

        public void AddRegion(string regionName, IRegion region)
        {
            if (_regions.TryAdd(regionName, region))
            {
                ToRegionsObservers(region);
                if (_buffer.TryPop(out var context))
                {
                    region.Activate(context);
                    ToNavigationObservers(context);
                    _buffer.Clear();
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
    }
}
