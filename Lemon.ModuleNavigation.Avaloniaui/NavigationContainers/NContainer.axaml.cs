using Avalonia;
using Avalonia.Controls;
using Lemon.ModuleNavigation.Avaloniaui.ViewModels;

namespace Lemon.ModuleNavigation.Avaloniaui.NavigationContainers;

public partial class NContainer : UserControl, IObserver<NavigationContext>
{
    private readonly IDisposable? _disposable;
    public NContainer()
    {
        InitializeComponent();
        _disposable = this.GetObservable(NavigationContextProperty)
            .Subscribe(this);
    }
    public static readonly StyledProperty<NavigationContext> NavigationContextProperty =
        AvaloniaProperty.Register<NContainer, NavigationContext>(nameof(NavigationContext));
    public NavigationContext NavigationContext
    {
        get => GetValue(NavigationContextProperty);
        set => SetValue(NavigationContextProperty, value);
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(NavigationContext value)
    {
        //
    }
}