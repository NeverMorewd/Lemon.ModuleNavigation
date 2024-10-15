using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui.Containers
{
    public class NContainer : ContentControl
    {
        public NContainer()
        {
            Bind(ContentProperty,
                new Binding(nameof(NavigationContext) 
                + "." 
                + nameof(NavigationContext.CurrentModule)));
            ContentTemplate = new FuncDataTemplate<IModule>((m, np) =>
            {
                return NavigationContext.CreateNewView(m) as Control;
            });
        }
        public static readonly StyledProperty<NavigationContext> NavigationContextProperty =
            AvaloniaProperty.Register<NContainer, NavigationContext>(nameof(NavigationContext));
        public NavigationContext NavigationContext
        {
            get => GetValue(NavigationContextProperty);
            set => SetValue(NavigationContextProperty, value);
        }
    }
}
