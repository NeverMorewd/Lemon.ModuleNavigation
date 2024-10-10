using Avalonia;
using Avalonia.ReactiveUI;
using Lemon.Hosting.AvaloniauiDesktop;
using Lemon.ModuleNavigation;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.Toolkit.Log;
using Lemon.Toolkit.Models;
using Lemon.Toolkit.Modules;
using Lemon.Toolkit.Services;
using Lemon.Toolkit.Shells;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Versioning;

namespace Lemon.Toolkit
{
    internal class Program
    {
        internal readonly static ConsoleService _consoleService = new();
        [STAThread]
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macos")]
        public static void Main(string[] args)
        {
            Console.SetOut(_consoleService);
            Console.SetError(_consoleService);
            Console.WriteLine("====𝕃𝕖𝕞𝕠𝕟====");
            var hostBuilder = Host.CreateApplicationBuilder();

            // config IConfiguration
            hostBuilder.Configuration
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .AddInMemoryCollection();

            // logger
            hostBuilder.Logging.ClearProviders();
            hostBuilder.Services.AddLogging(builder =>
            {
                var miniLevel = LogLevel.Debug;
                builder.SetMinimumLevel(miniLevel);
                builder.AddProvider(new UILoggerProvider("UILogger", 
                    _consoleService,
                    miniLevel));
            });
            
            // services
            hostBuilder.Services.AddSingleton(_consoleService);
            hostBuilder.Services.AddSingleton<TopLevelService>();
            hostBuilder.Services.AddSingleton<ShellService>();
            hostBuilder.Services.AddSingleton<IObservable<ShellParamModel>>(sp=>sp.GetRequiredService<ShellService>());
            hostBuilder.Services.AddSingleton<IObserver<ShellParamModel>>(sp => sp.GetRequiredService<ShellService>());
            
            // modules
            hostBuilder.Services.AddModule<HomeModule>();
            hostBuilder.Services.AddModule<FileInspectorModule>();
            hostBuilder.Services.AddModule<FileComparerModule>();
            hostBuilder.Services.AddModule<TestModule>();
            hostBuilder.Services.AddModulesBuilder();

            // navigation
            hostBuilder.Services.AddSingleton<NavigationService>();
            hostBuilder.Services.AddSingleton<INavigationService<IModule>>(sp => sp.GetRequiredService<NavigationService>());
            //
            hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(ConfigAvaloniaAppBuilder);
            hostBuilder.Services.AddMainWindow<MainWindow, MainWindowViewModel>();
            RunApp(hostBuilder, args);

        }
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macos")]
        private static void RunApp(HostApplicationBuilder hostBuilder, string[] args)
        {
            var appHost = hostBuilder.Build();
            appHost.RunAvaloniauiApplication<MainWindow>(args);
        }
        public static AppBuilder ConfigAvaloniaAppBuilder(AppBuilder appBuilder)
            => appBuilder
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }
}
