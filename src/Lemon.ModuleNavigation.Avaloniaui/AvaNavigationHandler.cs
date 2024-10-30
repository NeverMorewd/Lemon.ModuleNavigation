using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public class AvaNavigationHandler : NavigationHandler
    {
        private readonly Dictionary<(string, string), IView> _viewCache;
        public AvaNavigationHandler(IModuleNavigationService<IModule> navigationService,
            IViewNavigationService viewNavigationService,
            IEnumerable<IModule> modules,
            IServiceProvider serviceProvider)
            : base(navigationService,
                  viewNavigationService,
                  modules,
                  serviceProvider)
        {
            ViewContainers = [];
            _viewCache = [];
        }

        public Dictionary<string, Control> ViewContainers
        {
            get;
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
            var context = new NavigationContext(viewName,
                                                containerName,
                                                requestNew,
                                                navigationParameters);
            var container = ViewContainers[containerName];
            if (container is ContentControl contentControl)
            {
                if (!requestNew
                    && contentControl.Content is not null
                    && contentControl.Content is NavigationContext current
                    && NavigationContext.Comparer.Equals(current, context))
                {
                    return;
                }
                else
                {
                    contentControl.Content = context;
                    contentControl.ContentTemplate ??= GetDataTemplate();
                }
            }
            else if (container is TabControl tabControl)
            {
                tabControl.ContentTemplate ??= GetDataTemplate();
                var targetItem = tabControl.Items.FirstOrDefault(i =>
                {
                    if (i is NavigationContext current)
                    {
                        return NavigationContext.Comparer.Equals(current, context);
                    }
                    return false;
                });
                if (!requestNew && targetItem != null)
                {
                    tabControl.SelectedItem = targetItem;
                }
                else
                {
                    tabControl.Items.Add(context);
                    tabControl.SelectedIndex = tabControl.Items.Count - 1;
                }
            }
            else if (container is ItemsControl itemsControl)
            {
                itemsControl.ItemTemplate ??= GetDataTemplate();
                var targetItem = itemsControl.Items.FirstOrDefault(i =>
                {
                    if (i is NavigationContext current)
                    {
                        return NavigationContext.Comparer.Equals(current, context);
                    }
                    return false;
                });
                if (!requestNew && targetItem != null)
                {
                    itemsControl.ScrollIntoView(targetItem);
                }
                else
                {
                    itemsControl.Items.Add(context);
                    /// https://github.com/AvaloniaUI/Avalonia/issues/17347
                    Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);
                    itemsControl.ScrollIntoView(itemsControl.Items.Count - 1);
                }
            }
            Dispatcher.UIThread.RunJobs(DispatcherPriority.Loaded);
        }


        private IDataTemplate GetDataTemplate()
        {
            return new FuncDataTemplate<NavigationContext>((context, np) =>
            {
                if (context == null)
                {
                    return default;
                }
                if (context.RequestNew || !_viewCache.TryGetValue((context.ContainerName, context.TargetName), out IView? view))
                {
                    view = ServiceProvider.GetRequiredKeyedService<IView>(context.TargetName);

                    var viewFullName = view.GetType().FullName;

                    context.Uri = new Uri($"avares://{viewFullName}.axaml");
                    var viewModel = ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.TargetName);
                    viewModel.OnNavigatedTo(context);
                    if (viewModel.IsNavigationTarget(context))
                    {
                        view.DataContext = viewModel;
                    }
                    else
                    {
                        return default;
                    }
                    _viewCache.TryAdd((context.ContainerName, context.TargetName), view);
                }
                return view as Control;
            });
        }
    }
}
