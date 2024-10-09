using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.LogicalTree;
using Lemon.ModuleNavigation.Avaloniaui.ViewModels;

namespace Lemon.ModuleNavigation.Avaloniaui.NavigationContainers
{
    public class TabContainer : TabControl, IObserver<NavigationContext>
    {
        private readonly IDisposable? _disposable;

        public TabContainer() 
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
           AvaloniaProperty.Register<TabContainer, NavigationContext>(nameof(NavigationContext));
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

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            _disposable?.Dispose();
            base.OnDetachedFromLogicalTree(e);
        }
    }
}
