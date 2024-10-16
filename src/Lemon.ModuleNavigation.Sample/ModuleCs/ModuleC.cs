using Lemon.ModuleNavigation.Avaloniaui;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleCs
{
    public class ModuleC : AvaModule<ViewC, ViewModelC>
    {
        public ModuleC(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool LoadOnDemand => true;
        public override bool AllowMultiple => true;
        public override string? Alias => $"{base.Alias}:{nameof(AllowMultiple)}";
        public override void Initialize()
        {
            base.Initialize();
            Console.WriteLine($"Initialize:{nameof(ModuleC)}");
        }
    }
}
