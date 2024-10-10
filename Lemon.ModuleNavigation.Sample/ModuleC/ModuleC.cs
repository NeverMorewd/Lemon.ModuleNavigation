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
    }
}
