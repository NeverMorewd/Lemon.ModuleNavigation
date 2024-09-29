using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Xaml.Interactivity;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.Toolkit.Behaviors
{
    public class ItemsControlAutoScrollBehavior : Behavior<ItemsControl>
    {
        private ItemsControl? _currentControl;

        [RequiresUnreferencedCode("OnAttached")]
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is not { } itemsControl) return;
            _currentControl = itemsControl;
            _currentControl.ItemsView.CollectionChanged += CollectionChangedHandler;
        }

        private void CollectionChangedHandler(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add) return;
            var scrollViewer = _currentControl.FindLogicalAncestorOfType<ScrollViewer>(includeSelf: false);
            scrollViewer?.ScrollToEnd();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (_currentControl != null)
            {
                _currentControl.ItemsView.CollectionChanged -= CollectionChangedHandler;
            }
        }
    }
}
