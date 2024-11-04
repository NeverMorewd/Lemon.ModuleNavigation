using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Lemon.ModuleNavigation.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions
{
    public class ItemsRegion : AvaloniauiRegion
    {
        private readonly ItemsControl _itemsControl;
        public ItemsRegion(ItemsControl itemsControl)
        {
            _itemsControl = itemsControl;
            Contexts = [];
            Contexts.CollectionChanged += ViewContents_CollectionChanged;
            _itemsControl.ItemTemplate = RegionTemplate;
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
                        _itemsControl.Items.Remove(e.OldItems);
                    }
                }
            }
        }
    }
}
