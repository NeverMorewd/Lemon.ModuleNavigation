using Lemon.ModuleNavigation.Abstracts;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Core
{

    public class RegionManager : IRegionManager
    {
        private readonly Dictionary<string, IRegion> _regions = [];
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentStack<NavigationContext> _buffer = [];
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
            }
            else
            {
                _buffer.Push(context);
            }
        }

        public void AddRegion(string regionName, IRegion region)
        {
            _regions.Add(regionName, region);
            if (_buffer.TryPop(out var context))
            {
                region.Activate(context);
                _buffer.Clear();
            }
        }

        public IRegion? GetRegion(string regionName)
        {
            _regions.TryGetValue(regionName, out var region);
            return region;
        }
    }
}
