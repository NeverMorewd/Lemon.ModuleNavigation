using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
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
        ViewNameCache = [];
        Contexts = [];
        RegionTemplate = CreateRegionDataTemplate();
        Contexts.CollectionChanged += Contexts_CollectionChanged;
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
    public string Name
    {
        get;
    }

    public ObservableCollection<NavigationContext> Contexts
    {
        get;
    }

    public DataTemplate? RegionTemplate
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

    protected abstract IView? ResolveView(NavigationContext context);

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
