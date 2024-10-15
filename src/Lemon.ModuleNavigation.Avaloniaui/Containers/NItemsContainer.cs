using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui.Containers
{
    public class NItemsContainer : ItemsControl, IObserver<IModule>
    {
        private readonly IDisposable _disposable;
        public NItemsContainer()
        {
            Bind(ActiveItemProperty,
                new Binding(nameof(NavigationContext) + "." + nameof(NavigationContext.CurrentModule))
                {
                    Mode = BindingMode.TwoWay
                });
            Bind(ItemsSourceProperty,
                new Binding(nameof(NavigationContext) + "." + nameof(NavigationContext.ActiveModules)));

            ItemTemplate = new FuncDataTemplate<IModule>((m, np) =>
            {
                return NavigationContext.CreateNewView(m) as Control;
            });
            _disposable = this.GetObservable(ActiveItemProperty).Subscribe(this);
        }
        protected override Type StyleKeyOverride => typeof(ItemsControl);

        public static readonly StyledProperty<NavigationContext> NavigationContextProperty =
          AvaloniaProperty.Register<NItemsContainer, NavigationContext>(nameof(NavigationContext));
        public NavigationContext NavigationContext
        {
            get => GetValue(NavigationContextProperty);
            set => SetValue(NavigationContextProperty, value);
        }

        public static readonly StyledProperty<IModule> ActiveItemProperty =
          AvaloniaProperty.Register<NItemsContainer, IModule>(nameof(ActiveItem));
        public IModule ActiveItem
        {
            get => GetValue(ActiveItemProperty);
            set => SetValue(ActiveItemProperty, value);
        }

        public void OnCompleted()
        {
            //throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            //throw new NotImplementedException();
        }

        public void OnNext(IModule value)
        {
            ScrollIntoView(value);
        }
        protected override void OnUnloaded(Avalonia.Interactivity.RoutedEventArgs e)
        {
            _disposable?.Dispose();
        }
    }
}
