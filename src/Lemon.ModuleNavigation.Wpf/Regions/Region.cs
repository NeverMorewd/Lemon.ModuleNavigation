using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Lemon.ModuleNavigation.Wpf.Regions;

public abstract class Region : IRegion
{
    public Region(string name)
    {
        Name = name;
        Current = new();
        ViewCache = [];
        Contexts = [];
        RegionContentTemplate = CreateRegionDataTemplate();
        Contexts.CollectionChanged += Contexts_CollectionChanged;
    }
    protected ConcurrentItem<(IView View, INavigationAware NavigationAware)> Current
    {
        get;
    }
    public string Name
    {
        get;
    }
    protected ConcurrentDictionary<NavigationContext, IView> ViewCache
    {
        get;
    }

    protected ConcurrentDictionary<string, IView> ViewNameCache
    {
        get
        {
            return new(Contexts
                    .GroupBy(kv => kv.ViewName)
                    .Select(group => new KeyValuePair<string, IView>(
                        group.Key,
                        group.Last().View!)));
        }
    }
    public ObservableCollection<NavigationContext> Contexts
    {
        get;
    }

    public DataTemplate? RegionContentTemplate
    {
        get;
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

    protected IView? ResolveView(NavigationContext context)
    {
        var view = context.View;
        if (view is null)
        {
            view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.ViewName);
            var navigationAware = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.ViewName);

            if (Current.TryTakeData(out var previousData))
            {
                previousData.NavigationAware.OnNavigatedFrom(context);
            }

            view.DataContext = navigationAware;
            navigationAware.OnNavigatedTo(context);
            context.Alias = navigationAware.Alias;
            if (navigationAware is ICanUnload canUnloadNavigationAware)
            {
                canUnloadNavigationAware.RequestUnload += () =>
                {
                    DeActivate(context);
                };
            }
            Current.SetData((view, navigationAware));
            context.View = view;
            ViewCache.AddOrUpdate(context, view, (key, value) => view);
        }
        return view;
    }

    protected virtual void WhenContextsAdded(IEnumerable<NavigationContext> contexts)
    {

    }
    protected virtual void WhenContextsRemoved(IEnumerable<NavigationContext> contexts)
    {

    }
    private void Contexts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            if (e.NewItems is not null)
            {
                WhenContextsAdded(e.NewItems.Cast<NavigationContext>());
            }
        }
        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            if (e.OldItems is not null)
            {
                WhenContextsRemoved(e.OldItems.Cast<NavigationContext>());
            }
        }
    }
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
                return _region.ResolveView(context);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
