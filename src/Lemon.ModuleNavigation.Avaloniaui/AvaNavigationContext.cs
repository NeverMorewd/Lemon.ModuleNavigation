using Avalonia.Controls;
using Avalonia.Styling;
using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public class AvaNavigationContext : NavigationContext
    {
        public AvaNavigationContext(INavigationService<IModule> navigationService, 
            IEnumerable<IModule> modules,
            IEnumerable<IView> views,
            IServiceProvider serviceProvider)
            : base(navigationService, 
                  modules, 
                  serviceProvider)
        {
            NContainers = [];
        }

        public Dictionary<string, Control> NContainers
        {
            get;
        }

        public override void OnNavigateTo(string containerName, string viewName)
        {
            base.OnNavigateTo(containerName, viewName);
        }
        public override void OnNavigateTo<TView>(string containerName)
        {
            var view = ServiceProvider.GetRequiredKeyedService<TView>(containerName);
            var container = NContainers[containerName];
            
        }
        private void HandleContainerControl(Control container,IView view)
        {

        }
    }
}
