using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public class AvaNavigationContext : NavigationContext
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
            var container = ViewContainers[containerName];
            ContainerHandleCore(container, viewName, requestNew);
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
        private void ContainerHandleCore(Control container, 
            string viewName, 
            bool requestNew = false)
        {
            if (container is ContentControl contentControl)
            {
                SetDataTemplate(contentControl.ContentTemplate);
                if (!requestNew 
                    && contentControl.Content is not null
                    && contentControl.Content.ToString() == viewName)
                {
                    return;
                }
                else
                {
                    contentControl.Content = viewName;
                }
            }
            else if (container is TabControl tabControl)
            {
                SetDataTemplate(tabControl.ContentTemplate);
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
                SetDataTemplate(itemsControl.ItemTemplate);
                if (!requestNew && itemsControl.Items.Contains(viewName))
                {
                    itemsControl.ScrollIntoView(viewName);
                    if (itemsControl is ListBox listBox)
                    {
                        listBox.SelectedItem = viewName;
                    }
                }
                else
                {

                    itemsControl.Items.Add(viewName);
                    /// https://github.com/AvaloniaUI/Avalonia/issues/17347
                    var scrollViewer = itemsControl.FindLogicalAncestorOfType<ScrollViewer>(includeSelf: false);
                    scrollViewer?.ScrollToEnd();
                }
            }
        }


        private void SetDataTemplate(IDataTemplate? dataTemplate)
        {
            if (dataTemplate == null)
            {
                dataTemplate = new FuncDataTemplate<object>((m, np) =>
                {
                    return ServiceProvider.GetRequiredKeyedService<UserControl>(m);
                });
            }

        }
    }
}
