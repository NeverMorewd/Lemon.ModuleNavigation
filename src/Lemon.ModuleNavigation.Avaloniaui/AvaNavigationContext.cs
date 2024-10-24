using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public class AvaNavigationContext : NavigationContext
    {
        public AvaNavigationContext(INavigationService<IModule> navigationService,
            IViewNavigationService viewNavigationService,
            IEnumerable<IModule> modules,
            IServiceProvider serviceProvider)
            : base(navigationService,
                  viewNavigationService,
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
            var container = NContainers[containerName];
            ContainerHandleCore<TView>(container);


        }
        private void ContainerHandleCore<TView>(Control container) where TView : notnull
        {
            if (container is ContentControl contentControl)
            {
                contentControl.Content = this;
                contentControl.ContentTemplate = new FuncDataTemplate<object>((m, np) =>
                {
                    return ServiceProvider.GetRequiredService<TView>() as Control;
                });
            }
        }
    }
}
