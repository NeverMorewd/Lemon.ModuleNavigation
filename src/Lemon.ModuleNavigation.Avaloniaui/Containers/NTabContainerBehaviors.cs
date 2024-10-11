using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Lemon.ModuleNavigation.Abstracts;
using System.Collections.Concurrent;
using System.Linq;

namespace Lemon.ModuleNavigation.Avaloniaui.Containers
{
    /// <summary>
    /// NTabContainerBehaviors
    /// </summary>
    public class NTabContainerBehaviors
    {
        private static readonly HashSet<AvaloniaObject> _targets = [];
        static NTabContainerBehaviors()
        {
            
        }

        #region CanUnloadProperty
        /// <summary>
        /// Support Button Only
        /// </summary>
        public static readonly AttachedProperty<bool> CanUnloadProperty =
               AvaloniaProperty.RegisterAttached<NTabContainerBehaviors, Button, bool>("CanUnload", 
                   defaultValue: true,
                   coerce: CoerceCanUnload);

        private static bool CoerceCanUnload(AvaloniaObject targetObject, bool currentValue)
        {
            if (targetObject is Button button)
            {
                button.IsVisible = currentValue;
                if (currentValue)
                {
                    if (!_targets.Contains(button))
                    {
                        _targets.Add(button);
                        button.Unloaded += Button_Unloaded;
                        button.AddHandler(Button.ClickEvent, CloseTab, RoutingStrategies.Bubble, true);
                    }
                }
                return currentValue;
            }
            return false;
        }

        private static void Button_Unloaded(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (_targets.Contains(button))
                {
                    button.Unloaded -= Button_Unloaded;
                    button.RemoveHandler(Button.ClickEvent, CloseTab);
                    _targets.Remove(button);
                }
            }
        }

        public static bool GetCanUnload(Control control)
        {
            return control.GetValue(CanUnloadProperty);
        }
        public static void SetCanUnload(Control control, bool value)
        {
            control.SetValue(CanUnloadProperty, value);
        }
        #endregion
        private static void CloseTab(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var tabItem = FindParentOfType<TabItem>(button) ?? throw new InvalidOperationException($"There is no 'TabItem' found in parents of {button}");
                var tabContainer = FindParentOfType<NTabContainer>(tabItem);
                if (tabContainer != null)
                {
                    IModule item = tabItem.DataContext as IModule ?? throw new InvalidOperationException($"The DataContext of tabItem is not derived from IModule");
                    if (item.CanUnload)
                    {
                        tabContainer?.NavigationContext.ActiveModules.Remove(item);
                    }
                }
            }
        }
        private static T? FindParentOfType<T>(Visual target) where T : Visual
        {
            var parent = target.GetVisualParent();
            while (parent != null && parent is not T)
            {
                parent = parent.GetVisualParent();
            }
            return parent as T;
        }
    }
}
