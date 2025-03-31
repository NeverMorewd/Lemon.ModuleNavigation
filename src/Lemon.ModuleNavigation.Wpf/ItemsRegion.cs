using Lemon.ModuleNavigation.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Lemon.ModuleNavigation.Wpf;

public class ItemsRegion : Region
{
    private readonly ItemsControl _itemsControl;
    public ItemsRegion(ItemsControl itemsControl, string name)
    {
        _itemsControl = itemsControl;
        _itemsControl.ItemTemplate = RegionTemplate;
        Contexts = [];
        Contexts.CollectionChanged += ViewContents_CollectionChanged;
        Name = name;
    }
    public override string Name
    {
        get;
    }
    public object? SelectedItem
    {
        get
        {
            if (_itemsControl is Selector selecting)
            {
                return selecting.SelectedItem;
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                if (_itemsControl is Selector selecting)
                {
                    selecting.SelectedItem = value;
                }
            }
        }
    }

    public override ObservableCollection<NavigationContext> Contexts
    {
        get;
    }

    public override void ScrollIntoView(int index)
    {
        _itemsControl.ScrollIntoView(index);
    }
    public override void ScrollIntoView(NavigationContext item)
    {
        _itemsControl.ScrollIntoView(item);
    }
    public override void Activate(NavigationContext target)
    {
        if (!target.RequestNew)
        {
            var targetContext = Contexts.FirstOrDefault(context =>
            {
                if (NavigationContext.ViewNameComparer.Equals(target, context))
                {
                    return true;
                }
                return false;
            });
            if (targetContext == null)
            {
                Contexts.Add(target);
                SelectedItem = target;
            }
            else
            {
                SelectedItem = targetContext;
            }
        }
        else
        {
            Contexts.Add(target);
            SelectedItem = target;
        }
        ScrollIntoView((SelectedItem as NavigationContext)!);
    }
    public override void DeActivate(string viewName)
    {
        Contexts.Remove(Contexts.Last(c => c.TargetViewName == viewName));
    }
    public override void DeActivate(NavigationContext navigationContext)
    {
        Contexts.Remove(navigationContext);
    }
    public void Add(NavigationContext item)
    {
        Contexts.Add(item);
    }
    private void ViewContents_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    _itemsControl.Items.Add(item);
                }
            }
        }
        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    _itemsControl.Items.Remove(item);
                }
            }
        }
    }

    protected override IView? ResolveView(NavigationContext context)
    {
        bool needNewView = !ViewCache.TryGetValue(context.Guid, out IView? view);

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
            ViewCache[context.Guid] = view;
        }

        return view;
    }
}
