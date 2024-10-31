using Lemon.ModuleNavigation.Abstracts;

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

        public void RequestNavigate(string containerName, string viewName, bool requestNew, NavigationParameters? parameters = null)
        {
            if (_containers.TryGetValue(containerName, out var container))
            {
                var context = new NavigationContext(viewName, containerName, _serviceProvider, requestNew, parameters);
                container.Activate(context);
            }
        }

        public void AddContainer(string containerName, INavigationContainer container)
        {
            _containers.Add(containerName, container);
        }

        public INavigationContainer? GetContainer(string containerName)
        {
            _containers.TryGetValue(containerName, out var container);
            return container;
        }
    }

}
