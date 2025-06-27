using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Lemon.ModuleNavigation.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace Lemon.ModuleNavigation.Avaloniaui;

public class NavigationExtension
{
    private static readonly HashSet<AvaloniaObject> _targets = [];

    #region RegionNameProperty
    public static readonly AttachedProperty<string> RegionNameProperty =
           AvaloniaProperty.RegisterAttached<NavigationExtension, Control, string>("RegionName",
               coerce: CoerceRegionName);

    private static string CoerceRegionName(AvaloniaObject targetObject, string currentValue)
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
                    if (control.DataContext is IAsyncServiceAware asyncServerAware)
                    {
                        var serviceProvider = asyncServerAware!.ServiceProvider;
                        var handler = serviceProvider.GetRequiredService<IAsyncViewNavigationHandler>();
                        handler.AsyncRegionManager.AddRegion(currentValue, control.ToAsyncRegion(currentValue));
                    }
                    else if (control.DataContext is IServiceAware navigationProvider)
                    {
                        var serviceProvider = navigationProvider!.ServiceProvider;
                        var handler = serviceProvider.GetRequiredService<INavigationHandler>();
                        handler.RegionManager.AddRegion(currentValue, control.ToRegion(currentValue));
                    }
                    control.Loaded -= LoadedHandler;
                }
                control.Loaded += LoadedHandler;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(RegionNameProperty)} supports {nameof(ContentControl)} and {nameof(ItemsControl)} Only");
            }
        }
        return currentValue;
    }

    public static string GetRegionName(Control control)
    {
        return control.GetValue(RegionNameProperty);
    }
    public static void SetRegionName(Control control, string value)
    {
        control.SetValue(RegionNameProperty, value);
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
            if (!string.IsNullOrEmpty(GetRegionName(control)))
            {
                throw new InvalidOperationException($"The {control} have a {nameof(RegionNameProperty)} already.");
            }
            void LoadedHandler(object? sender, RoutedEventArgs e)
            {
                if (control.DataContext is IServiceAware navigationContextProvider)
                {
                    var serviceProvider = navigationContextProvider.ServiceProvider;
                    var handler = serviceProvider.GetRequiredService<INavigationHandler>();
                    SetBinding(control, handler, currentValue);
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
               defaultValue: false,
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

    private static void UnloadModule(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var view = button.FindLogicalAncestorOfType<IView>(includeSelf: false);
            if (view != null)
            {
            }
            var tabItem = button.FindLogicalAncestorOfType<TabItem>(includeSelf: false) ?? throw new InvalidOperationException($"There is no 'TabItem' found in parents of {button}");
            var tabContainer = tabItem.FindLogicalAncestorOfType<TabControl>(includeSelf: false);
            if (tabContainer != null)
            {
                if (tabItem.DataContext is IModule item)
                {
                    if (item.CanUnload)
                    {
                        if (tabContainer.DataContext is IServiceAware serviceAware)
                        {
                            var handler = serviceAware.ServiceProvider.GetRequiredService<INavigationHandler>();
                            handler.ModuleManager.ActiveModules.Remove(item);
                        }
                    }
                }
            }
        }
    }
    #endregion

    private static void SetBinding(Control control,
        INavigationHandler navigationHandler,
        string regionName)
    {
        if (control is TabControl tabControl)
        {
            tabControl.Bind(SelectingItemsControl.SelectedItemProperty,
                                new Binding(nameof(IModuleManager.CurrentModule))
                                {
                                    Mode = BindingMode.TwoWay,
                                    Source = navigationHandler.ModuleManager
                                });
            tabControl.Bind(ItemsControl.ItemsSourceProperty,
                                new Binding(nameof(IModuleManager.ActiveModules))
                                {
                                    Source = navigationHandler.ModuleManager
                                });

            tabControl.ContentTemplate = new FuncDataTemplate<IModule>((m, np) =>
            {
                if (m == null)
                {
                    return null;
                }
                return navigationHandler.ModuleManager.GetOrCreateView(m, regionName) as Control;
            });
        }
        else if (control is ItemsControl itemsControl)
        {
            if (navigationHandler.ModuleManager is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(IModuleManager.CurrentModule))
                    {
                        if (navigationHandler.ModuleManager.CurrentModule != null)
                        {
                            //https://github.com/AvaloniaUI/Avalonia/issues/17349
                            if (itemsControl is SelectingItemsControl selecting)
                            {
                                selecting.SelectedItem = navigationHandler.ModuleManager.CurrentModule;
                            }
                            itemsControl.ScrollIntoView(navigationHandler.ModuleManager.CurrentModule);
                        }
                    }
                };
            }
            itemsControl.Bind(ItemsControl.ItemsSourceProperty,
                        new Binding(nameof(IModuleManager.ActiveModules))
                        {
                            Source = navigationHandler.ModuleManager
                        });

            itemsControl.ItemTemplate = new FuncDataTemplate<IModule>((m, np) =>
            {
                if (m == null)
                {
                    return null;
                }
                return navigationHandler.ModuleManager.GetOrCreateView(m, regionName) as Control;
            });
        }
        else if (control is ContentControl contentControl)
        {
            contentControl.Bind(ContentControl.ContentProperty,
                                    new Binding(nameof(NavigationHandler.ModuleManager.CurrentModule))
                                    {
                                        Source = navigationHandler.ModuleManager
                                    });

            contentControl.ContentTemplate = new FuncDataTemplate<IModule>((m, np) =>
            {
                if (m == null)
                {
                    return null;
                }
                return navigationHandler.ModuleManager.GetOrCreateView(m, regionName) as Control;
            });
        }
    }
}
