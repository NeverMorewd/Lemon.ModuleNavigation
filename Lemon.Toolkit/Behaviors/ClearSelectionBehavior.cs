using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.Toolkit.Behaviors
{
    public class ClearSelectionBehavior : Behavior<ListBox>
    {
        private ListBox? _listBox;
        private IDisposable? _disposable;
        public ClearSelectionBehavior()
        {

        }

        public static readonly StyledProperty<bool> GoClearProperty =
            AvaloniaProperty.Register<Behavior, bool>(nameof(GoClear));
        public bool GoClear
        {
            get => GetValue(GoClearProperty);
            set => SetValue(GoClearProperty, value);
        }
        private void OnGoClearPropertyChanged(bool newValue)
        {
            if (newValue)
            {
                _listBox?.UnselectAll();
            }
        }

        [RequiresUnreferencedCode("OnAttached")]
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is ListBox listBox)
            {
                _listBox = listBox;
            }
            _disposable = this.GetObservable(GoClearProperty)
                              .Subscribe(OnGoClearPropertyChanged);
        }

        protected override void OnDetaching()
        {
            //_listBox?.UnselectAll();
            base.OnDetaching();
            _disposable?.Dispose();
        }

    }
}
