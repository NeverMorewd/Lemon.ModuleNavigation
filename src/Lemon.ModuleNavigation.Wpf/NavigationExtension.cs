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
                        handler.RegionManager.AddRegion(value, control.ToRegion(value));
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

}
