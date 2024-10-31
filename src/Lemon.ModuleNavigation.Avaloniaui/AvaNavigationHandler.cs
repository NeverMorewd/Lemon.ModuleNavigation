using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public class AvaNavigationHandler : NavigationHandler
    {
        private readonly Dictionary<(string, string), IView> _viewCache;
        public AvaNavigationHandler(IModuleNavigationService<IModule> navigationService,
            IViewNavigationService viewNavigationService,
            IEnumerable<IModule> modules,
            INavigationContainerManager navigationContainerManager,
            IServiceProvider serviceProvider)
            : base(navigationService,
                  viewNavigationService,
                  modules,
                  navigationContainerManager,
                  serviceProvider)
        {
            _viewCache = [];
        }

        public override void OnNavigateTo(string containerName, 
            string viewName, 
            bool requestNew = false)
        {
            ContainerHandleCore(containerName, viewName, null, requestNew);
        }
        public override void OnNavigateTo(string containerName,
            string viewName,
            NavigationParameters navigationParameters,
            bool requestNew = false)
        {
            ContainerHandleCore(containerName, viewName, navigationParameters, requestNew);
        }
        private void ContainerHandleCore(string containerName,
            string viewName,
            NavigationParameters? navigationParameters,
            bool requestNew = false)
        {

            ContainerManager.RequestNavigate(containerName, viewName, requestNew, navigationParameters);
            //var context = new NavigationContext(viewName,
            //                        containerName,
            //                        requestNew,
            //                        navigationParameters);
            //var container = ContainerManager.GetContainer(containerName);
            //if (container is ContentControl contentControl)
            //{
            //    if (!requestNew
            //        && contentControl.Content is not null
            //        && contentControl.Content is NavigationContext current
            //        && NavigationContext.ViewNameComparer.Equals(current, context))
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        contentControl.Content = context;
            //        contentControl.ContentTemplate ??= GetDataTemplate();
            //    }
            //}
            //else if (container is TabControl tabControl)
            //{
            //    tabControl.ContentTemplate ??= GetDataTemplate();
            //    var targetItem = GetTargetItem(tabControl.Items, context);
            //    if (!requestNew && targetItem != null)
            //    {
            //        tabControl.SelectedItem = targetItem;
            //    }
            //    else
            //    {
            //        tabControl.Items.Add(context);
            //        tabControl.SelectedIndex = tabControl.Items.Count - 1;
            //    }
            //}
            //else if (container is ItemsControl itemsControl)
            //{
            //    itemsControl.ItemTemplate ??= GetDataTemplate();
            //    var targetItem = GetTargetItem(itemsControl.Items, context);
            //    if (!requestNew && targetItem != null)
            //    {
            //        itemsControl.ScrollIntoView(targetItem);
            //    }
            //    else
            //    {
            //        itemsControl.Items.Add(context);
            //        /// https://github.com/AvaloniaUI/Avalonia/issues/17347
            //        Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);
            //        itemsControl.ScrollIntoView(itemsControl.Items.Count - 1);
            //    }
            //}
        }

        private object? GetTargetItem(ItemCollection itemCollection, 
            NavigationContext navigationContext)
        {
            return itemCollection.FirstOrDefault(i =>
            {
                if (i is NavigationContext current)
                {
                    return NavigationContext.ViewNameComparer.Equals(current, navigationContext);
                }
                return false;
            });
        }
    }
}
