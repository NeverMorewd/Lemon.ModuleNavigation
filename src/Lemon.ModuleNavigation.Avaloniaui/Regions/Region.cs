using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions;

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

    public IDataTemplate? RegionContentTemplate
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
    private IDataTemplate CreateRegionDataTemplate()
    {
        return new FuncDataTemplate<NavigationContext>((context, np) =>
        {
            if (context is null)
            {
                return null;
            }
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
            return view as Control;
        });
    }
}
