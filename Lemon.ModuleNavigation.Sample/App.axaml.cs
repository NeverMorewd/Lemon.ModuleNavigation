using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Lemon.ModuleNavigation.Sample.ViewModels;
using Lemon.ModuleNavigation.Sample.Views;

namespace Lemon.ModuleNavigation.Sample;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();
    }
}
