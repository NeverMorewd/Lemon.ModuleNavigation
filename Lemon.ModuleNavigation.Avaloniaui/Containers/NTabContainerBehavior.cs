using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui.Containers
{
    public class NTabContainerBehavior : AvaloniaObject
    {
        //private readonly IDisposable _disposed;
        static NTabContainerBehavior()
        {
            CanCloseProperty.Changed.Subscribe(new CanClosePropertyObserver());
        }

        #region CanCloseProperty
        public static readonly AttachedProperty<bool> CanCloseProperty =
               AvaloniaProperty.RegisterAttached<NTabContainerBehavior, Control, bool>("CanClose");

        public static bool GetCanClose(Control control)
        {
            return control.GetValue(CanCloseProperty);
        }
        public static void SetCanClose(Control control, bool value)
        {
            control.SetValue(CanCloseProperty, value);
        }
        #endregion

        private static T? FindParentOfType<T>(Visual target) where T : Visual
        {
            var parent = target.GetVisualParent();
            while (parent != null && parent is not T)
            {
                parent = parent.GetVisualParent();
            }
            return parent as T;
        }

        private class CanClosePropertyObserver : IObserver<AvaloniaPropertyChangedEventArgs<bool>>
        {
            public void OnCompleted()
            {
                
            }

            public void OnError(Exception error)
            {
                
            }

            public void OnNext(AvaloniaPropertyChangedEventArgs<bool> value)
            {
                if (value.NewValue.HasValue
                   && value.NewValue.Value is bool canClose
                   && canClose)
                {
                    var sender = (Control)value.Sender;
                    sender.AddHandler(Button.ClickEvent, CloseTab, RoutingStrategies.Bubble, true);
                }
            }

            private void CloseTab(object? sender, RoutedEventArgs e)
            {
                if (sender is Button button)
                {
                    var tabItem = FindParentOfType<TabItem>(button) ?? throw new InvalidOperationException($"There is no 'TabItem' found in parents of {button}");
                    var tabContainer = FindParentOfType<NTabContainer>(tabItem);
                    if (tabContainer != null)
                    {
                        IModule item = tabItem.DataContext as IModule ?? throw new InvalidOperationException($"The DataContext of tabItem is not derived from IModule");
                        tabContainer?.NavigationContext.ActiveModules.Remove(item);
                    }
                }
            }
        }
    }

}
