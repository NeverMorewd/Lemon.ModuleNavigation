using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace Lemon.ModuleNavigation.Avaloniaui.Containers
{
    public class NTabContainer : TabControl, IObserver<NavigationContext>
    {
        private readonly IDisposable? _disposable;

        public NTabContainer() 
        {
            Bind(SelectedItemProperty,
                new Binding(nameof(NavigationContext) + "." + nameof(NavigationContext.CurrentModule))
                {
                    Mode = BindingMode.TwoWay
                });
            Bind(ItemsSourceProperty,
                new Binding(nameof(NavigationContext) + "." + nameof(NavigationContext.ActiveModules)));

            _disposable = this.GetObservable(NavigationContextProperty)
                              .Subscribe(this);
        }
        protected override Type StyleKeyOverride => typeof(TabControl);

        public static readonly StyledProperty<NavigationContext> NavigationContextProperty =
           AvaloniaProperty.Register<NTabContainer, NavigationContext>(nameof(NavigationContext));
        public NavigationContext NavigationContext
        {
            get => GetValue(NavigationContextProperty);
            set => SetValue(NavigationContextProperty, value);
        }

        public void OnCompleted()
        {
            //
        }

        public void OnError(Exception error)
        {
            //
        }

        public void OnNext(NavigationContext? value)
        {
            if (value != null)
            {
                SelectedItem = value.CurrentModule;
            }
        }
        protected override void OnUnloaded(Avalonia.Interactivity.RoutedEventArgs e)
        {
            base.OnUnloaded(e);
            _disposable?.Dispose();
        }
    }
}
