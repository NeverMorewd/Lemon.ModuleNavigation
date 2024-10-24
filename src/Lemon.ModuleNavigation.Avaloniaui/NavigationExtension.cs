using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Lemon.ModuleNavigation.Abstracts;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public class NavigationExtension
    {
        private static readonly ConcurrentDictionary<string, Control> _viewContainerNamesCaches = [];
        private static readonly ConcurrentDictionary<string, Control> _moduleContainerNamesCaches = [];
        private static readonly HashSet<AvaloniaObject> _targets = [];
        #region ViewContainerNameProperty
        public static readonly AttachedProperty<string> ViewContainerNameProperty =
               AvaloniaProperty.RegisterAttached<NavigationExtension, Control, string>("ViewContainerName",
                   coerce: CoerceViewContainerName);

        private static string CoerceViewContainerName(AvaloniaObject targetObject, string currentValue)
        {
            if (targetObject is Control control)
            {
                if (control is ContentControl || control is ItemsControl)
                {
                    if (!string.IsNullOrEmpty(GetModuleContainerName(control)))
                    {
                        throw new InvalidOperationException($"The {control} have a {nameof(ModuleContainerNameProperty)} already..");
                    }
                    void LoadedHandler(object? sender, RoutedEventArgs e)
                    {
                        var navigationContextProvider = control.DataContext as INavigationContextProvider;
                        var navigationContext = navigationContextProvider!.NavigationContext;
                        if (navigationContext is AvaNavigationContext context)
                        {
                            context.ViewContainers.TryAdd(currentValue, control);
                        }
                        control.Loaded -= LoadedHandler;
                    }
                    control.Loaded += LoadedHandler;
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(ViewContainerNameProperty)} supports {nameof(ContentControl)} and {nameof(ItemsControl)} Only");
                }
            }
            return currentValue;
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

        #region ModuleContainerName
        public static readonly AttachedProperty<string> ModuleContainerNameProperty =
               AvaloniaProperty.RegisterAttached<NavigationExtension, Control, string>("ModuleContainerName",
                   validate: ValidateModuleContainerName,
                   coerce: CoerceModuleContainerName);

        private static bool ValidateModuleContainerName(string arg)
        {
            return true;
        }

        private static string CoerceModuleContainerName(AvaloniaObject targetObject, string currentValue)
        {
            if (string.IsNullOrEmpty(currentValue))
            {
                return currentValue;
            }
            if (targetObject is Control control)
            {
                if (!string.IsNullOrEmpty(GetViewContainerName(control)))
                {
                    throw new InvalidOperationException($"The {control} have a {nameof(ViewContainerNameProperty)} already.");
                }
                void LoadedHandler(object? sender, RoutedEventArgs e)
                {
                    var navigationContextProvider = control.DataContext as INavigationContextProvider;
                    var navigationContext = navigationContextProvider!.NavigationContext;
                    if (navigationContext is AvaNavigationContext context)
                    {
                        SetBinding(control, context);
                    }
                    control.Loaded -= LoadedHandler;
                }
                control.Loaded += LoadedHandler;
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
        #endregion
        #region CanUnloadProperty
        public static readonly AttachedProperty<bool> CanUnloadProperty =
               AvaloniaProperty.RegisterAttached<NavigationExtension, Button, bool>("CanUnload",
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
                        button.AddHandler(Button.ClickEvent, UnloadModule, RoutingStrategies.Bubble, true);
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
                    button.RemoveHandler(Button.ClickEvent, UnloadModule);
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
        private static void UnloadModule(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var tabItem = button.FindLogicalAncestorOfType<TabItem>(includeSelf: false) ?? throw new InvalidOperationException($"There is no 'TabItem' found in parents of {button}");
                var tabContainer = tabItem.FindLogicalAncestorOfType<TabControl>(includeSelf: false);
                if (tabContainer != null)
                {
                    IModule item = tabItem.DataContext as IModule ?? throw new InvalidOperationException($"The DataContext of tabItem is not derived from IModule");
                    if (item.CanUnload)
                    {
                        if (tabContainer.DataContext is INavigationContextProvider navigationContextProvider)
                        {
                            navigationContextProvider.NavigationContext.ActiveModules.Remove(item);
                        }
                    }
                }
            }
        }

        private static void SetBinding(Control control, AvaNavigationContext navigationContext)
        {
            if (control is TabControl tabControl)
            {
                tabControl.Bind(SelectingItemsControl.SelectedItemProperty,
                                    new Binding(nameof(NavigationContext) 
                                    + "." 
                                    + nameof(NavigationContext.CurrentModule))
                                    {
                                        Mode = BindingMode.TwoWay
                                    });
                tabControl.Bind(ItemsControl.ItemsSourceProperty,
                                    new Binding(nameof(NavigationContext) 
                                    + "." 
                                    + nameof(NavigationContext.ActiveModules)));

                tabControl.ContentTemplate = new FuncDataTemplate<IModule>((m, np) =>
                {
                    if (m == null)
                    {
                        return null;
                    }
                    return navigationContext.CreateNewView(m) as Control;
                });
            }
            else if (control is ItemsControl itemsControl)
            {
                if (navigationContext is INotifyPropertyChanged npc)
                {
                    npc.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName == nameof(NavigationContext.CurrentModule))
                        {
                            if (navigationContext.CurrentModule != null)
                            {
                                itemsControl.ScrollIntoView(navigationContext.CurrentModule);
                                if (itemsControl is SelectingItemsControl selecting)
                                {
                                    selecting.SelectedItem = navigationContext.CurrentModule;
                                }
                            }
                        }
                    };
                }
                itemsControl.Bind(ItemsControl.ItemsSourceProperty,
                            new Binding(nameof(NavigationContext)
                            + "."
                            + nameof(NavigationContext.ActiveModules)));

                itemsControl.ItemTemplate = new FuncDataTemplate<IModule>((m, np) =>
                {
                    if (m == null)
                    {
                        return null;
                    }
                    return navigationContext.CreateNewView(m) as Control;
                });
            }
            else if (control is ContentControl contentControl)
            {
                contentControl.Bind(ContentControl.ContentProperty,
                                        new Binding(nameof(NavigationContext) 
                                        + "."
                                        + nameof(NavigationContext.CurrentModule)));

                contentControl.ContentTemplate = new FuncDataTemplate<IModule>((m, np) =>
                {
                    if (m == null)
                    {
                        return null;
                    }
                    return navigationContext.CreateNewView(m) as Control;
                });
            }
        }
    }
}
