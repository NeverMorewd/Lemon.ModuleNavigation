using Lemon.ModuleNavigation.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Lemon.ModuleNavigation.Wpf.Regions;

public class ItemsRegion : Region, IItemsRegionDataContext<DataTemplate>
{
    private readonly ItemsControl _itemsControl;
    public ItemsRegion(string name, ItemsControl itemsControl) : base(name)
    {
        _itemsControl = itemsControl;
        SetBindingItemTemplate();
        SetBindingSelectedItem();
        SetBindingItemsSource();
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

    private DataTemplate? _itemsTemplate;
    public DataTemplate? ItemTemplate
    {
        get => _itemsTemplate;
        set
        {
            _itemsTemplate = value;
            OnPropertyChanged();
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    public override void ScrollIntoView(int index)
    {
        _itemsControl.ScrollIntoView(index);
    }
    public override void ScrollIntoView(NavigationContext item)
    {
        _itemsControl.ScrollIntoView(item);
    }

    /// <summary>
    /// When Views with same ViewName were found, the earliest one will be picked.
    /// </summary>
    /// <param name="target"></param>
    public override void Activate(NavigationContext target)
    {
        try
        {
            if (ViewCache.TryGetValue(target, out IView? accurateView))
            {
                target.View = accurateView;
                SelectedItem = target;
                return;
            }
            var context = Contexts.FirstOrDefault(c => c.ViewName == target.ViewName);
            if (context is not null
                && context.View is not null
                && context.View.DataContext is INavigationAware navigationAware
                && navigationAware.IsNavigationTarget(target))
            {
                SelectedItem = context;
                return;
            }
            Contexts.Add(target);
            SelectedItem = target;
        }
        finally
        {
            ScrollIntoView((SelectedItem as NavigationContext)!);
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

    protected virtual void SetBindingItemsSource()
    {
        BindingOperations.SetBinding(_itemsControl, ItemsControl.ItemsSourceProperty, new Binding
        {
            Source = this,
            Path = new PropertyPath(nameof(Contexts)),
        });
    }
    protected virtual void SetBindingItemTemplate()
    {
        ItemTemplate = RegionContentTemplate;
        BindingOperations.SetBinding(_itemsControl, ItemsControl.ItemTemplateProperty, new Binding
        {
            Source = this,
            Path = new PropertyPath(nameof(ItemTemplate)),
        });
    }
    protected virtual void SetBindingSelectedItem()
    {
        if (_itemsControl is Selector selector)
        {
            BindingOperations.SetBinding(selector, Selector.SelectedItemProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(SelectedItem)),
                Mode = BindingMode.TwoWay
            });
        }
    }
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
