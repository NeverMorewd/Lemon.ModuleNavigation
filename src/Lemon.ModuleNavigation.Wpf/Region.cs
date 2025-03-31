using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Lemon.ModuleNavigation.Wpf;

public abstract class Region : IRegion, INotifyPropertyChanged
{
    public Region()
    {
        Current = new();
        ViewCache = [];
        ViewNameCache = [];
        RegionTemplate = CreateRegionDataTemplate();
    }

    protected ConcurrentDictionary<Guid, IView> ViewCache
    {
        get;
    }
    protected ConcurrentDictionary<string, IView> ViewNameCache
    {
        get;
    }
    protected ConcurrentItem<(IView View, INavigationAware NavigationAware)> Current
    {
        get;
    }
    public abstract string Name
    {
        get;
    }

    public abstract ObservableCollection<NavigationContext> Contexts
    {
        get;
    }

    public DataTemplate? RegionTemplate
    {
        get;
        private set;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public virtual void ScrollIntoView(int index)
    {
        throw new NotImplementedException();
    }
    public virtual void ScrollIntoView(NavigationContext item)
    {
        throw new NotImplementedException();
    }
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
        bool needNewView = !ViewNameCache.TryGetValue(context.TargetViewName, out IView? view);

        if (!needNewView)
        {
            if (view!.DataContext is INavigationAware navigationAware
                && !navigationAware.IsNavigationTarget(context))
            {
                needNewView = true;
            }
        }

        if (needNewView)
        {
            view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.TargetViewName);
            var navigationAware = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.TargetViewName);

            if (Current.TryTakeData(out var previousData))
            {
                previousData.NavigationAware.OnNavigatedFrom(context);
            }

            view.DataContext = navigationAware;
            navigationAware.OnNavigatedTo(context);
            Current.SetData((view, navigationAware));
            ViewNameCache[context.TargetViewName] = view;
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
            if (value is NavigationContext context)
            {
                var view = _region.ResolveView(context);
                _region.ViewCache.AddOrUpdate(context.Guid, view!, (key, oldValue) => view!);
                return view;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
