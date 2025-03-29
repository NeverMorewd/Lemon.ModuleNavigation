using Lemon.ModuleNavigation.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Lemon.ModuleNavigation.Wpf;

public class NavigationExtension
{
    //private static readonly ConcurrentDictionary<string, Control> _moduleContainerNamesCaches = [];
    private static readonly HashSet<DependencyObject> _targets = [];
    #region RegionNameProperty
    public static readonly DependencyProperty RegionNameProperty =
        DependencyProperty.RegisterAttached(
            "RegionName",
            typeof(string),
            typeof(NavigationExtension),
            new PropertyMetadata(null, new PropertyChangedCallback(RegionNameChangedCallback)));

    private static void RegionNameChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Control control)
        {
            if (control is ContentControl || control is ItemsControl)
            {
                void LoadedHandler(object? sender, RoutedEventArgs e)
                {
                    if (control.DataContext is IServiceAware navigationProvider)
                    {
                        var serviceProvider = navigationProvider!.ServiceProvider;
                        var handler = serviceProvider.GetRequiredService<INavigationHandler>();
                        var value = GetRegionName(control);
                        handler.RegionManager.AddRegion(value, control.ToContainer(value));
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
    }

    public static void SetRegionName(Control element, string value)
    {
        element.SetValue(RegionNameProperty, value);
    }

    public static string GetRegionName(Control element)
    {
        return (string)element.GetValue(RegionNameProperty);
    }
    #endregion

    #region CanUnload -- ongoing
    public static readonly DependencyProperty CanUnloadProperty =
        DependencyProperty.RegisterAttached(
            "CanUnload", typeof(bool), typeof(NavigationExtension),
            new PropertyMetadata(true, OnCanUnloadChanged));

    public static bool GetCanUnload(DependencyObject obj) => (bool)obj.GetValue(CanUnloadProperty);
    public static void SetCanUnload(DependencyObject obj, bool value) => obj.SetValue(CanUnloadProperty, value);

    private static void OnCanUnloadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Button button)
        {
            bool newValue = (bool)e.NewValue;
            button.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;

            if (newValue)
            {
                if (!_targets.Contains(button))
                {
                    _targets.Add(button);
                    button.Unloaded += Button_Unloaded;
                    button.Click += UnloadModule;
                }
            }
            else
            {
                if (_targets.Contains(button))
                {
                    button.Unloaded -= Button_Unloaded;
                    button.Click -= UnloadModule;
                    _targets.Remove(button);
                }
            }
        }
    }

    private static void Button_Unloaded(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && _targets.Contains(button))
        {
            button.Unloaded -= Button_Unloaded;
            button.Click -= UnloadModule;
            _targets.Remove(button);
        }
    }

    private static void UnloadModule(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var tabItem = FindAncestor<TabItem>(button) ??
                          throw new InvalidOperationException($"No 'TabItem' found in parents of {button}");

            var tabContainer = FindAncestor<TabControl>(tabItem);
            if (tabContainer != null)
            {
                if (tabItem.DataContext is INavigationAware item)
                {
                    if (tabContainer.DataContext is IServiceAware serviceAware)
                    {
                        var handler = serviceAware.ServiceProvider.GetRequiredService<INavigationHandler>();
                    }
                }
            }
        }
    }

    private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T ancestor)
                return ancestor;
            current = LogicalTreeHelper.GetParent(current);
        }
        return null;
    }
    #endregion

}
