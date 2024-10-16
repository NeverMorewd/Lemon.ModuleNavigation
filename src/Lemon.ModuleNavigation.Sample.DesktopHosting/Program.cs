using Avalonia;
using Avalonia.ReactiveUI;
using Lemon.Hosting.AvaloniauiDesktop;
using Lemon.ModuleNavigation.Sample.ModuleAs;
using Lemon.ModuleNavigation.Sample.ModuleBs;
using Lemon.ModuleNavigation.Sample.ModuleCs;
using Lemon.ModuleNavigation.Sample.ViewModels;
using Lemon.ModuleNavigation.Sample.Views;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Versioning;

namespace Lemon.ModuleNavigation.Sample.Desktop;

class Program
{
    [STAThread]
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("macos")]
    public static void Main(string[] args)
    {
        var hostBuilder = Host.CreateApplicationBuilder();

        // module navigation
        hostBuilder.Services.AddNavigationSupport();
        hostBuilder.Logging.ClearProviders();
        hostBuilder.Logging.AddConsole();
        hostBuilder.Logging.SetMinimumLevel(LogLevel.Debug);
        // modules
        hostBuilder.Services.AddModule<ModuleA>();
        hostBuilder.Services.AddModule<ModuleB>();
        hostBuilder.Services.AddModule<ModuleC>();

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
