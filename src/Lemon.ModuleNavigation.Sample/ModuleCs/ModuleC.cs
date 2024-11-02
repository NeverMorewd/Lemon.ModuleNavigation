using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ModuleCs.Services;
using Lemon.ModuleNavigation.Sample.ModuleCs.ViewModels;
using Lemon.ModuleNavigation.Sample.ModuleCs.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Lemon.ModuleNavigation.Sample.ModuleCs
{
    //[ServiceDescriptors(
    //    new List<ServiceDescription> { new ServiceDescription(typeof(SomeService), ServiceLifetime.Singleton) }, 
    //    new List<NavigationDescription> { new NavigationDescription(typeof(SubView01), typeof(SubViewModel01), ServiceLifetime.Singleton) })
    //]
    [ServiceDescriptors(
        [typeof(SomeService), typeof(SomeService)],
        [ServiceLifetime.Singleton, ServiceLifetime.Transient],
        [null, "myKey"]
    )]
    public class ModuleC : IModule
    {
        private readonly ILogger _logger;
        public ModuleC(IServiceProvider serviceProvider, ILogger<ModuleC> logger) : base(serviceProvider)
        {
            _logger = logger;
        }
        public bool LoadOnDemand => true;
        public bool ForceNew => true;
        public string Alias => $"";

        public string Key => throw new NotImplementedException();

        public bool IsInitialized => throw new NotImplementedException();

        public bool CanUnload => throw new NotImplementedException();

        public bool IsActivated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Type ViewType => throw new NotImplementedException();

        public Type ViewModelType => throw new NotImplementedException();

        public IEnumerable<KeyValuePair<Type, Type>> ViewTypes => throw new NotImplementedException();

        public IEnumerable<Type> ServiceTypes => throw new NotImplementedException();

        public void ConfigService(IServiceCollection serviceDescriptors)
        {
            //serviceDescriptors.adds
        }
        public void ConfigView(IServiceCollection serviceDescriptors)
        {
            
        }

        public void Initialize()
        {
            Console.WriteLine($"Initialize:{nameof(ModuleC)}");
        }
    }
}
