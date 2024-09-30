using Avalonia;
using Avalonia.ReactiveUI;
using Lemon.Hosting.AvaloniauiDesktop;
using Lemon.Toolkit.Framework;
using Lemon.Toolkit.Framework.Abstracts;
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
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddProvider(new UILoggerProvider("UILogger", _consoleService));
            });
            
            // services
            hostBuilder.Services.AddSingleton(_consoleService);
            hostBuilder.Services.AddSingleton<TopLevelService>();
            hostBuilder.Services.AddSingleton<ShellService>();
            hostBuilder.Services.AddSingleton<IObservable<ShellParamModel>>(sp=>sp.GetRequiredService<ShellService>());
            hostBuilder.Services.AddSingleton<IObserver<ShellParamModel>>(sp => sp.GetRequiredService<ShellService>());
            
            // modules
            hostBuilder.Services.AddTabModule<HomeModule>();
            hostBuilder.Services.AddTabModule<FileInspectorModule>();
            hostBuilder.Services.AddTabModule<CompareModule>();
            hostBuilder.Services.AddTabModulesBuilder();

            Subject<IModule> navigationService = new();
            hostBuilder.Services.AddSingleton(navigationService.AsObservable());
            hostBuilder.Services.AddSingleton(navigationService.AsObserver());

            RunApp(hostBuilder, args);

        }
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macos")]
        private static void RunApp(HostApplicationBuilder hostBuilder, string[] args)
        {
            hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(ConfigAvaloniaAppBuilder);
            hostBuilder.Services.AddMainWindow<MainWindow, MainWindowViewModel>();
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
