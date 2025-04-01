using Lemon.ModuleNavigation.Abstractions;
using Microsoft.Extensions.DependencyInjection;
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
            view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.ViewName);
            var navigationAware = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.ViewName);

            if (Current.TryTakeData(out var previousData))
            {
                previousData.NavigationAware.OnNavigatedFrom(context);
            }

            view.DataContext = navigationAware;
            navigationAware.OnNavigatedTo(context);
            navigationAware.RequestUnload += () =>
            {
                DeActivate(context);
            };
            Current.SetData((view, navigationAware));
            ViewCache[context.Guid] = view;
        }

        return view;
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
        ItemTemplate = RegionTemplate;
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
