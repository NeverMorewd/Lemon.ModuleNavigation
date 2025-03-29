using Avalonia.Controls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Lemon.ModuleNavigation.Avaloniaui;

public class TabRegion : AvaloniauiRegion
{
    private readonly TabControl _tabControl;
    public TabRegion(TabControl tabControl, string name)
    {
        _tabControl = tabControl;
        _tabControl.ContentTemplate = RegionTemplate;
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
            return _tabControl.SelectedItem;
        }
        set
        {
            _tabControl.SelectedItem = value;
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
    public override void DeActivate(NavigationContext target)
    {
        Contexts.Remove(target);
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
                    _tabControl.Items.Remove(e.OldItems);
                }
            }
        }
    }
}
