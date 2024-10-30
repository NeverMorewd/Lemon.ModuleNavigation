using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Core
{

    public class NavigationContainerManager : INavigationContainerManager
    {
        private readonly Dictionary<string, INavigationContainer> _containers = [];
        private readonly IServiceProvider _serviceProvider;
        public NavigationContainerManager(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }
        public void RegisterViewWithRegion(string regionName, Func<IView> viewFactory)
        {
            if (!_containers.TryGetValue(regionName, out var region))
            {
                region = new NavigationContainer();
                _containers[regionName] = region;
            }

            region.Views.Add(viewFactory());
        }

        public void RequestNavigate(string regionName, string viewName, NavigationParameters parameters = null)
        {
            if (_containers.TryGetValue(regionName, out var region))
            {
               var view = _serviceProvider.GetRequiredKeyedService<IView>(viewName);
               region.Views.Add(view);
            }
        }

        public INavigationContainer CreateRegion(string regionName)
        {
            var region = new NavigationContainer();
            _containers[regionName] = region;
            return region;
        }

        public INavigationContainer? GetRegion(string regionName)
        {
            _containers.TryGetValue(regionName, out var region);
            return region;
        }
    }

}
