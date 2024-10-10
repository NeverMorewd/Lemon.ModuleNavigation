using Avalonia;
using Avalonia.ReactiveUI;
using Lemon.Hosting.AvaloniauiDesktop;
using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.ModuleNavigation.Sample.ModuleAs;
using Lemon.ModuleNavigation.Sample.ModuleBs;
using Lemon.ModuleNavigation.Sample.ModuleCs;
using Lemon.ModuleNavigation.Sample.ViewModels;
using Lemon.ModuleNavigation.Sample.Views;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.Versioning;

namespace Lemon.ModuleNavigation.Sample.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("macos")]
    public static void Main(string[] args)
    {
        var hostBuilder = Host.CreateApplicationBuilder();

        // navigation
        hostBuilder.Services.AddNavigationContext();

        hostBuilder.Services.AddModule<ModuleA>();
        hostBuilder.Services.AddModule<ModuleB>();
        hostBuilder.Services.AddModule<ModuleC>();
        //
        hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(BuildAvaloniaApp);
        hostBuilder.Services.AddMainWindow<MainWindow, MainViewModel>();
        var appHost = hostBuilder.Build();
        appHost.RunAvaloniauiApplication<MainWindow>(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp(AppBuilder appBuilder)
    {
        return appBuilder
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}
