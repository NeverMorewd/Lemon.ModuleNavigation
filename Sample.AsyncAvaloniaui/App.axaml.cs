using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Sample.AsyncAvaloniaui.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using Lemon.ModuleNavigation.SampleViewModel;

namespace Sample.AsyncAvaloniaui;

public partial class App : Application
{
    public IServiceProvider? AppServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAsyncNavigationSupport()
                .AddSingleton<MainWindow>()
                .AddSingleton<AsyncMainWindowViewModel>()
                .AddAsyncView<ViewAlpha, AsyncViewAlphaViewModel>(nameof(ViewAlpha))
                .AddAsyncView<ViewBeta, AsyncViewBetaViewModel>(nameof(ViewBeta));
        AppServiceProvider = services.BuildServiceProvider();

        var viewModel = AppServiceProvider.GetRequiredService<AsyncMainWindowViewModel>();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                var window = AppServiceProvider.GetRequiredService<MainWindow>();
                window.DataContext = viewModel;
                desktop.MainWindow = window;
                break;
        }
        base.OnFrameworkInitializationCompleted();
    }
}