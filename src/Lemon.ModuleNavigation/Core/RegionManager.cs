using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Core
{

    public class RegionManager : IRegionManager
    {
        private readonly Dictionary<string, IRegion> _containers = [];
        private readonly IServiceProvider _serviceProvider;
        public RegionManager(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public void RequestNavigate(string containerName, string viewName, bool requestNew, NavigationParameters? parameters = null)
        {
            if (_containers.TryGetValue(containerName, out var container))
            {
                var context = new NavigationContext(viewName, containerName, _serviceProvider, requestNew, parameters);
                container.Activate(context);
            }
        }

        public void AddContainer(string containerName, IRegion container)
        {
            _containers.Add(containerName, container);
        }

        public IRegion? GetContainer(string containerName)
        {
            _containers.TryGetValue(containerName, out var container);
            return container;
        }
    }

}
