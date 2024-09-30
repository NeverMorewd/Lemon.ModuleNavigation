using Avalonia;
using Avalonia.Markup.Xaml;
using Lemon.Hosting.Modularization.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.Hosting.Modularization.Avaloniaui
{
    public class SmApp : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IModule> _modules;
        public SmApp(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
            _modules = _serviceProvider.GetRequiredService<IEnumerable<IModule>>();
        }

        public virtual void ModulesInialize()
        {
            
        }
        public virtual void SmAppInitialize()
        {
            Initialize();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();
            ModulesInialize();
        }


        public override void Initialize()
        {
            base.Initialize();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
