using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lemon.ModuleNavigation.Avaloniaui.RegionsOld;

public class TabRegion : RegionBak, INotifyPropertyChanged
{
    private readonly TabControl _tabControl;
     public TabRegion(TabControl tabControl, string name)
    {
        _tabControl = tabControl;
        Contexts = [];
        //Contexts.CollectionChanged += ViewContents_CollectionChanged;


        _tabControl.Bind(SelectingItemsControl.SelectedItemProperty,
                               new Binding(nameof(SelectedItem))
                               {
                                   Mode = BindingMode.TwoWay,
                                   Source = this
                               });
        _tabControl.Bind(ItemsControl.ItemsSourceProperty,
                            new Binding(nameof(Contexts))
                            {
                                Source = this
                            });
        _tabControl.ContentTemplate = RegionTemplate;

        Name = name;
    }
    public override string Name
    {
        get;
    }
    private NavigationContext? _selectItem;
    public NavigationContext? SelectedItem
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
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
}
