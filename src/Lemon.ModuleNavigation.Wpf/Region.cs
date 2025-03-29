using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Lemon.ModuleNavigation.Wpf;

public abstract class Region : IRegion
{
    private readonly ConcurrentDictionary<string, IView> _viewCache = new();
    private readonly ConcurrentItem<(IView View, INavigationAware NavigationAware)> _current = new();

    public Region()
    {
        RegionTemplate = CreateRegionDataTemplate();
    }

    public abstract string Name { get; }
    public abstract ObservableCollection<NavigationContext> Contexts { get; }

    public DataTemplate? RegionTemplate { get; set; }

    public abstract void Activate(NavigationContext target);
    public abstract void DeActivate(NavigationContext target);

    private DataTemplate CreateRegionDataTemplate()
    {
        var dataTemplate = new DataTemplate(typeof(NavigationContext));
        FrameworkElementFactory factory = new(typeof(ContentPresenter));
        factory.SetBinding(ContentPresenter.ContentProperty, new System.Windows.Data.Binding()
        {
            Converter = new NavigationContextToViewConverter(this)
        });
        dataTemplate.VisualTree = factory;
        return dataTemplate;
    }

    public class NavigationContextToViewConverter : System.Windows.Data.IValueConverter
    {
        private readonly Region _region;
        public NavigationContextToViewConverter(Region region)
        {
            _region = region;
        }

        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is not NavigationContext context)
                return null;

            bool needNewView = context.RequestNew ||
                !_region._viewCache.TryGetValue(context.TargetViewName, out IView? view);

            if (needNewView)
            {
                view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.TargetViewName);
                var navigationAware = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.TargetViewName);

                if (_region._current.TryTakeData(out var previousData))
                {
                    previousData.NavigationAware.OnNavigatedFrom(context);
                }

                if (!navigationAware.IsNavigationTarget(context))
                    return null;

                view.DataContext = navigationAware;
                navigationAware.OnNavigatedTo(context);
                _region._current.SetData((view, navigationAware));
                _region._viewCache.TryAdd(context.TargetViewName, view);
            }
            else
            {
                view = _region._viewCache[context.TargetViewName];
            }

            return view as Control;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


