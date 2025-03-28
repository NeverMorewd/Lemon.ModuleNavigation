using Lemon.ModuleNavigation.Wpf.Extensions;
using Lemon.ModuleNavigation.WpfSample.ViewModels;
using Lemon.ModuleNavigation.WpfSample.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Lemon.ModuleNavigation.WpfSample;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider? AppServiceProvider { get; private set; }
    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        services.AddWpfNavigationSupport()
                //.AddModule<ModuleA>()
                //.AddModule<ModuleB>()
                //.AddModule<ModuleC>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainWindowViewModel>()
                //.AddDialogWindow<CustomDialogWindow>(nameof(CustomDialogWindow))
                .AddView<ViewAlpha, ViewAlphaViewModel>(nameof(ViewAlpha))
                .AddView<ViewBeta, ViewBetaViewModel>(nameof(ViewBeta));

        AppServiceProvider = services.BuildServiceProvider();
        var window = AppServiceProvider.GetRequiredService<MainWindow>();
        var mainViewModel = AppServiceProvider.GetRequiredService<MainWindowViewModel>();
        window.DataContext = mainViewModel;
        base.OnStartup(e);

        window.Show();
        Application.Current.MainWindow = window;
    }
}

