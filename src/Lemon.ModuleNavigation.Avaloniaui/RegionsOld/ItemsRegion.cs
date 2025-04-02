using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Lemon.ModuleNavigation.Avaloniaui.RegionsOld;

public class ItemsRegion : RegionBak
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
            if (_itemsControl is SelectingItemsControl selecting)
            {
                return selecting.SelectedItem;
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                if (_itemsControl is SelectingItemsControl selecting)
                {
                    selecting.SelectedItem = value;
                }
                _itemsControl.ScrollIntoView(value);
            }
        }
    }

    public override ObservableCollection<NavigationContext> Contexts
    {
        get;
    }

    public void ScrollIntoView(int index)
    {
        _itemsControl.ScrollIntoView(index);
    }
    public void ScrollIntoView(NavigationContext item)
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
    }
    public override void DeActivate(string viewName)
    {
        Contexts.Remove(Contexts.Last(c => c.ViewName == viewName));
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
}
