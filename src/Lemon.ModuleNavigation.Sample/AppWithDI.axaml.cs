using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Lemon.ModuleNavigation.Sample.ModuleAs;
using Lemon.ModuleNavigation.Sample.ModuleBs;
using Lemon.ModuleNavigation.Sample.ModuleCs;
using Lemon.ModuleNavigation.Sample.ViewModels;
using Lemon.ModuleNavigation.Sample.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lemon.ModuleNavigation.Sample;

public partial class AppWithDI : Application
{
    private IServiceProvider? _serviceProvider;

    public IServiceProvider? AppServiceProvider
    {
        get
        {
            return _serviceProvider;
        }
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddNavigationContext()
                .AddModule<ModuleA>()
                .AddModule<ModuleB>()
                .AddModule<ModuleC>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainView>()
                .AddSingleton<MainViewModel>();
        _serviceProvider = services.BuildServiceProvider();

        var viewModel = _serviceProvider.GetRequiredService<MainViewModel>();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                var window = _serviceProvider.GetRequiredService<MainWindow>();
                window.DataContext = viewModel;
                desktop.MainWindow = window;
                break;
            case ISingleViewApplicationLifetime singleView:
                var view = _serviceProvider.GetRequiredService<MainView>();
                view.DataContext = viewModel;
                singleView.MainView = view;
                break;
        }
        base.OnFrameworkInitializationCompleted();
    }
}
