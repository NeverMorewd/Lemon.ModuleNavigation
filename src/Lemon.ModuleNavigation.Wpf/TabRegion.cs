using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Lemon.ModuleNavigation.Wpf;

public class TabRegion : Region
{
    private readonly TabControl _tabControl;
    public TabRegion(TabControl tabControl, string name)
    {
        _tabControl = tabControl;
        _tabControl.ContentTemplate = RegionTemplate;
        Contexts = [];
        Contexts.CollectionChanged += ViewContents_CollectionChanged;
        Name = name;
        BindingOperations.SetBinding(_tabControl, Selector.SelectedItemProperty, new Binding
        {
            Source = this,
            Path = new PropertyPath(nameof(SelectedItem)),
            Mode = BindingMode.TwoWay
        });
        BindingOperations.SetBinding(_tabControl, ItemsControl.ItemsSourceProperty, new Binding
        {
            Source = this,
            Path = new PropertyPath(nameof(SelectedItem)),
            Mode = BindingMode.TwoWay
        });
    }
    public override string Name
    {
        get;
    }
    private object? _selectItem;
    public object? SelectedItem
    {
        get
        {
            return _selectItem;
        }
        set
        {
            _selectItem = value;
            OnPropertyChanged();
        }
    }
    public override ObservableCollection<NavigationContext> Contexts
    {
        get;
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
                    _tabControl.Items.Add(item);
                }
            }
        }
        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    _tabControl.Items.Remove(item);
                }
            }
        }
    }


    protected override IView? ResolveView(NavigationContext context)
    {
        bool needNewView = !ViewCache.TryGetValue(context.Guid, out IView? view);

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
