using Avalonia;
using Avalonia.Controls;

namespace Lemon.ModuleNavigation.Avaloniaui.Containers;

public partial class NContainer : UserControl
{
    public NContainer()
    {
        InitializeComponent();
    }
    public static readonly StyledProperty<NavigationContext> NavigationContextProperty =
        AvaloniaProperty.Register<NContainer, NavigationContext>(nameof(NavigationContext));
    public NavigationContext NavigationContext
    {
        get => GetValue(NavigationContextProperty);
        set => SetValue(NavigationContextProperty, value);
    }
}