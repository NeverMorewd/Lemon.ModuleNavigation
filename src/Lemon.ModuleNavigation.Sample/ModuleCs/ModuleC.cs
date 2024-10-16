using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.ModuleNavigation.Sample.ModuleCs.SubModules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lemon.ModuleNavigation.Sample.ModuleCs
{
    public class ModuleC : AvaModule<ViewC, ViewModelC>
    {
        private readonly IServiceProvider _subServiceProvider;
        public ModuleC(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ModuleServices = new ServiceCollection();
            ModuleServices.AddModule<SubModule01>();
            ModuleServices.AddModule<SubModule02>();
            ModuleServices.AddNavigationContext();
            //_advancedSp = ModuleServices.BuildAdvancedServiceProvider();

            _subServiceProvider = ModuleServices.BuildServiceProvider();
        }
        public IServiceCollection ModuleServices
        {
            get;
        }
        public override bool LoadOnDemand => true;
        public override bool AllowMultiple => true;
        public override string? Alias => $"{base.Alias}:{nameof(AllowMultiple)}";
        public override void Initialize()
        {
            base.Initialize();
            Console.WriteLine($"Initialize:{nameof(ModuleC)}");
            var subModules = _subServiceProvider.GetRequiredService<IEnumerable<IModule>>();
            foreach (var subModule in subModules) 
            {
                Debug.WriteLine(subModule.Key);
            }
        }
    }
}
