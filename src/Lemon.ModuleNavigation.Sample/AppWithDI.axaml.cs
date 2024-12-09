using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Lemon.ModuleNavigation.Avaloniaui.Extensions;
using Lemon.ModuleNavigation.Sample.ModuleAs;
using Lemon.ModuleNavigation.Sample.ModuleBs;
using Lemon.ModuleNavigation.Sample.ModuleCs;
using Lemon.ModuleNavigation.Sample.ViewModels;
using Lemon.ModuleNavigation.Sample.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lemon.ModuleNavigation.Sample;

public partial class AppWithDi : Application
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
        services.AddAvaNavigationSupport()
                .AddModule<ModuleA>()
                .AddModule<ModuleB>()
                .AddModule<ModuleC>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainView>()
                .AddSingleton<MainViewModel>()
                .AddAvaDialogWindow<CustomDialogWindow>(nameof(CustomDialogWindow))
                .AddView<ViewAlpha, ViewAlphaViewModel>(nameof(ViewAlpha))
                .AddView<ViewBeta, ViewBetaViewModel>(nameof(ViewBeta));
        AppServiceProvider = services.BuildServiceProvider();

        var viewModel = AppServiceProvider.GetRequiredService<MainViewModel>();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                var window = AppServiceProvider.GetRequiredService<MainWindow>();
                window.DataContext = viewModel;
                desktop.MainWindow = window;
                break;
            case ISingleViewApplicationLifetime singleView:
                var view = AppServiceProvider.GetRequiredService<MainView>();
                view.DataContext = viewModel;
                singleView.MainView = view;
                break;
        }
        base.OnFrameworkInitializationCompleted();
    }
}
