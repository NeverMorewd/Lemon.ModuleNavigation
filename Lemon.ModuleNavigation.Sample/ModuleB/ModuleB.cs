using System;
using Lemon.ModuleNavigation.Avaloniaui;

namespace Lemon.ModuleNavigation.Sample.ModuleB
{
    public class ModuleB : AvaModule<ViewB, ViewModelB>
    {
        public ModuleB(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool LoadOnDemand => true;
    }
}
