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
    public abstract void DeActivate(string viewName);
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

    protected virtual IView? ResolveView(NavigationContext context)
    {
        bool needNewView = !_viewCache.TryGetValue(context.TargetViewName, out IView? view);

        if (!needNewView)
        {
            var navigationAware = view!.DataContext as INavigationAware;
            if (navigationAware != null && !navigationAware.IsNavigationTarget(context))
            {
                needNewView = true; 
            }
        }

        if (needNewView)
        {
            view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.TargetViewName);
            var navigationAware = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.TargetViewName);

            if (_current.TryTakeData(out var previousData))
            {
                previousData.NavigationAware.OnNavigatedFrom(context);
            }

            view.DataContext = navigationAware;
            navigationAware.OnNavigatedTo(context);
            _current.SetData((view, navigationAware));
            _viewCache[context.TargetViewName] = view;
        }

        return view;
    }

    private class NavigationContextToViewConverter : System.Windows.Data.IValueConverter
    {
        private readonly Region _region;
        public NavigationContextToViewConverter(Region region)
        {
            _region = region;
        }

        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is NavigationContext context ? _region.ResolveView(context) as Control : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
