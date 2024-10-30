using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public class AvaNavigationContext : NavigationHandler
    {
        private readonly Dictionary<string, IDataTemplate> _contentDatas;
        public AvaNavigationContext(INavigationService<IModule> navigationService,
            IViewNavigationService viewNavigationService,
            IEnumerable<IModule> modules,
            IServiceProvider serviceProvider)
            : base(navigationService,
                  viewNavigationService,
                  modules,
                  serviceProvider)
        {
            ViewContainers = [];
            _contentDatas = [];
        }

        public Dictionary<string, Control> ViewContainers
        {
            get;
        }

        public override void OnNavigateTo(string containerName, 
            string viewName, 
            bool requestNew = false)
        {
            ContainerHandleCore(containerName, viewName, requestNew);
        }
        public override void OnNavigateTo(string containerName,
            string viewName,
            NavigationParameters navigationParameters,
            bool requestNew = false)
        {
            ContainerHandleCore(containerName, viewName, requestNew);
        }
        private void ContainerHandleCore(string containerName, 
            string viewName, 
            bool requestNew = false)
        {
            var container = ViewContainers[containerName];
            if (container is ContentControl contentControl)
            {
                if (!requestNew 
                    && contentControl.Content is not null
                    && contentControl.Content.ToString() == viewName)
                {
                    return;
                }
                else
                {
                    contentControl.Content = viewName;
                    contentControl.ContentTemplate ??= GetDataTemplate();
                }
            }
            else if (container is TabControl tabControl)
            {
                tabControl.ContentTemplate ??= GetDataTemplate();
                if (!requestNew && tabControl.Items.Contains(viewName))
                {
                    tabControl.SelectedItem = viewName;
                }
                else
                {
                    tabControl.Items.Add(viewName);
                    tabControl.SelectedIndex = tabControl.Items.Count - 1;

                }
            }
            else if (container is ItemsControl itemsControl)
            {
                itemsControl.ItemTemplate ??= GetDataTemplate();
                if (!requestNew && itemsControl.Items.Contains(viewName))
                {
                    itemsControl.ScrollIntoView(viewName);
                }
                else
                {
                    itemsControl.Items.Add(viewName);
                    /// https://github.com/AvaloniaUI/Avalonia/issues/17347
                    Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);
                    itemsControl.ScrollIntoView(itemsControl.Items.Count - 1);
                }
            }
        }


        private IDataTemplate GetDataTemplate()
        {
            return new FuncDataTemplate<object>((m, np) =>
            {
                if (m == null)
                {
                    return default;
                }
                // todo:bug
                var view = ServiceProvider.GetRequiredKeyedService<IView>(m);
                var viewModel = ServiceProvider.GetRequiredKeyedService<INavigationAware>(m);
                view.DataContext = viewModel;
                return view as Control;
            });
        }
    }
}
