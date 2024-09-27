using Avalonia;
using Avalonia.ReactiveUI;
using Lemon.Hosting.AvaloniauiDesktop;
using Lemon.Toolkit.Comparer.Services;
using Lemon.Toolkit.Comparer.ViewModels;
using Lemon.Toolkit.Comparer.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Versioning;

namespace Lemon.Toolkit.Comparer
{
    internal class Program
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
            var consoleService = new ConsoleService();
            Console.SetOut(consoleService);
            Console.SetError(consoleService);

            var hostBuilder = Host.CreateApplicationBuilder();

            // config IConfiguration
            hostBuilder.Configuration
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .AddInMemoryCollection();

            // config ILogger
            hostBuilder.Logging.ClearProviders();
            hostBuilder.Services.AddLogging(builder => builder.AddConsole());
            hostBuilder.Services.AddSingleton(consoleService);
            hostBuilder.Services.AddSingleton<TopLevelService>();

            #region app default
            RunApp(hostBuilder, args);
            #endregion

        }
        [SupportedOSPlatform("windows")]
        [SupportedOSPlatform("linux")]
        [SupportedOSPlatform("macos")]
        private static void RunApp(HostApplicationBuilder hostBuilder, string[] args)
        {
            hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(ConfigAvaloniaAppBuilder);
            hostBuilder.Services.AddMainWindow<MainWindow, MainWindowViewModel>();
            var appHost = hostBuilder.Build();
            // run app
            appHost.RunAvaloniauiApplication<MainWindow>(args);
        }
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder ConfigAvaloniaAppBuilder(AppBuilder appBuilder)
            => appBuilder
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }
}
