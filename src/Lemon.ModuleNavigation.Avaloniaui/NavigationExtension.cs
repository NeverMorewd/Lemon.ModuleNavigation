using Avalonia;
using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public class NavigationExtension
    {
        private static readonly ConcurrentDictionary<string, Control> _containerNamesCaches = [];
        #region ContainerNameProperty
        public static readonly AttachedProperty<string> ContainerNameProperty =
               AvaloniaProperty.RegisterAttached<NavigationExtension, Control, string>("ContainerName",
                   defaultValue: "",
                   coerce: CoerceContainerName);

        private static string CoerceContainerName(AvaloniaObject targetObject, string currentValue)
        {
            if (targetObject is ContentControl contentContainer)
            {
                if (!_containerNamesCaches.TryAdd(currentValue, contentContainer))
                {
                    throw new InvalidOperationException($"There is already a container named {currentValue}!");
                }
                contentContainer.Loaded += (s, e) =>
                {
                    var navigationContextProvider = contentContainer.DataContext as INavigationContextProvider;
                    var navigationContext = navigationContextProvider!.NavigationContext;
                    if (navigationContext is AvaNavigationContext context)
                    {
                        if (!context.NContainers.ContainsKey(currentValue))
                        {
                            context.NContainers.Add(currentValue, contentContainer);
                        }
                    }
                };

                //contentContainer.Bind(ContentControl.ContentProperty,
                //new Binding(nameof(NavigationContext)
                //+ "."
                //+ nameof(navigationContext.CurrentModule)));
                //contentContainer.ContentTemplate = new FuncDataTemplate<IModule>((m, np) =>
                //{
                //    if (m == null)
                //    {
                //        return null;
                //    }
                //    return navigationContext.CreateNewView(m) as Control;
                //});
                return currentValue;
            }
            else if (targetObject is ItemsControl itemsContainer)
            {
                if (!_containerNamesCaches.TryAdd(currentValue, itemsContainer))
                {
                    throw new InvalidOperationException($"There is already a container named {currentValue}!");
                }
                itemsContainer.Loaded += (s, e) =>
                {
                    var navigationContextProvider = itemsContainer.DataContext as INavigationContextProvider;
                    var navigationContext = navigationContextProvider!.NavigationContext;
                    if (navigationContext is AvaNavigationContext context)
                    {
                        if (!context.NContainers.ContainsKey(currentValue))
                        {
                            context.NContainers.Add(currentValue, itemsContainer);
                        }
                    }
                };
                return currentValue;
            }
            throw new InvalidOperationException("NContainerName support ContentControl or ItemsControl Only");
        }


        public static string GetContainerName(Control control)
        {
            return control.GetValue(ContainerNameProperty);
        }
        public static void SetContainerName(Control control, string value)
        {
            control.SetValue(ContainerNameProperty, value);
        }
        #endregion
    }
}
