using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Lemon.ModuleNavigation.Abstracts;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Avaloniaui.Containers
{
    public class NContainerExtension
    {
        private static readonly ConcurrentDictionary<string, Control> _containerNamesCaches = [];
        #region ContainerNameProperty
        public static readonly AttachedProperty<string> ContainerNameProperty =
               AvaloniaProperty.RegisterAttached<NContainerExtension, Control, string>("ContainerName",
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
                var navigationContextProvider = contentContainer.DataContext as INavigationContextProvider;
                var navigationContext = navigationContextProvider!.NavigationContext;
                contentContainer.Bind(ContentControl.ContentProperty,
                new Binding(nameof(NavigationContext)
                + "."
                + nameof(navigationContext.CurrentModule)));
                contentContainer.ContentTemplate = new FuncDataTemplate<IModule>((m, np) =>
                {
                    if (m == null)
                    {
                        return null;
                    }
                    return navigationContext.CreateNewView(m) as Control;
                });
                return currentValue;
            }
            else if (targetObject is ItemsControl itemsContainer)
            {
                if (!_containerNamesCaches.TryAdd(currentValue, itemsContainer))
                {
                    throw new InvalidOperationException($"There is already a container named {currentValue}!");
                }
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
