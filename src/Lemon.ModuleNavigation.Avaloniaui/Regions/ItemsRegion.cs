using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;
using Lemon.ModuleNavigation.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions;

public class ItemsRegion : Region, IItemsRegionDataContext<IDataTemplate>
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

    private IDataTemplate? _itemsTemplate;
    public IDataTemplate? ItemTemplate
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
        _itemsControl.Bind(ItemsControl.ItemsSourceProperty,
                               new Binding(nameof(Contexts))
                               {
                                   Source = this
                               });
    }
    protected virtual void SetBindingItemTemplate()
    {
        ItemTemplate = RegionContentTemplate;
        _itemsControl.Bind(ItemsControl.ItemTemplateProperty,
                       new Binding(nameof(ItemTemplate))
                       {
                           Source = this
                       });
    }
    protected virtual void SetBindingSelectedItem()
    {
        if (_itemsControl is SelectingItemsControl selector)
        {
            selector.Bind(SelectingItemsControl.SelectedItemProperty,
                new Binding(nameof(SelectedItem))
                {
                    Source = this,
                    Mode = BindingMode.TwoWay
                });
        }
    }
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
