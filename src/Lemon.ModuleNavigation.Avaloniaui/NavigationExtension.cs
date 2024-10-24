using Avalonia;
using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public class NavigationExtension
    {
        private static readonly ConcurrentDictionary<string, Control> _viewContainerNamesCaches = [];
        private static readonly ConcurrentDictionary<string, Control> _moduleContainerNamesCaches = [];
        #region ViewContainerNameProperty
        public static readonly AttachedProperty<string> ViewContainerNameProperty =
               AvaloniaProperty.RegisterAttached<NavigationExtension, Control, string>("ViewContainerName",
                   defaultValue: "",
                   coerce: CoerceViewContainerName);

        private static string CoerceViewContainerName(AvaloniaObject targetObject, string currentValue)
        {
            if (targetObject is Control control)
            {
                var otherName = GetModuleContainerName(control);
                if (!string.IsNullOrEmpty(otherName))
                {
                    throw new InvalidOperationException();
                }
            }
            if (targetObject is ContentControl contentContainer)
            {
                if (!_viewContainerNamesCaches.TryAdd(currentValue, contentContainer))
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
                return currentValue;
            }
            else if (targetObject is ItemsControl itemsContainer)
            {
                if (!_viewContainerNamesCaches.TryAdd(currentValue, itemsContainer))
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


        public static string GetViewContainerName(Control control)
        {
            return control.GetValue(ViewContainerNameProperty);
        }
        public static void SetViewContainerName(Control control, string value)
        {
            control.SetValue(ViewContainerNameProperty, value);
        }
        #endregion


        public static readonly AttachedProperty<string> ModuleContainerNameProperty =
       AvaloniaProperty.RegisterAttached<NavigationExtension, Control, string>("ModuleContainerName",
           defaultValue: "",
           validate: ValidateModuleContainerName,
           coerce: CoerceModuleContainerName);

        private static bool ValidateModuleContainerName(string arg)
        {
            return true;
        }

        private static string CoerceModuleContainerName(AvaloniaObject targetObject, string currentValue)
        {
            if (targetObject is Control control)
            {
                var otherName = GetViewContainerName(control);
                if (!string.IsNullOrEmpty(otherName))
                {
                    throw new InvalidOperationException();
                }
            }
            return currentValue;
        }


        public static string GetModuleContainerName(Control control)
        {
            return control.GetValue(ModuleContainerNameProperty);
        }
        public static void SetModuleContainerName(Control control, string value)
        {
            control.SetValue(ModuleContainerNameProperty, value);
        }
    }
}
