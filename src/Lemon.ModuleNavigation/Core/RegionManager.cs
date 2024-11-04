using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Core
{

    public class RegionManager : IRegionManager
    {
        private readonly Dictionary<string, IRegion> _regions = [];
        private readonly IServiceProvider _serviceProvider;
        public RegionManager(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public void RequestNavigate(string regionName, string viewName, bool requestNew, NavigationParameters? parameters = null)
        {
            if (_regions.TryGetValue(regionName, out var region))
            {
                var context = new NavigationContext(viewName, regionName, _serviceProvider, requestNew, parameters);
                region.Activate(context);
            }
            else
            {
                throw new KeyNotFoundException($"Can not find region {regionName}");
            }
        }

        public void AddRegion(string regionName, IRegion region)
        {
            _regions.Add(regionName, region);
        }

        public IRegion? GetRegion(string regionName)
        {
            _regions.TryGetValue(regionName, out var region);
            return region;
        }
    }

}
