using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace Lemon.ModuleNavigation.Avaloniaui.Containers
{
    public class NContainer : ContentControl
    {
        public NContainer()
        {
            Bind(ContentTemplateProperty,
                new Binding(nameof(NavigationContext) 
                + "." 
                + nameof(NavigationContext.CurrentModule) 
                + "." 
                + "ViewTemplate"));
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
