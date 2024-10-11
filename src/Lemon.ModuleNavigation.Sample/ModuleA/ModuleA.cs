using System;
using Lemon.ModuleNavigation.Avaloniaui;

namespace Lemon.ModuleNavigation.Sample.ModuleAs
{
    public class ModuleA : AvaModule<ViewA, ViewModelA>
    {
        public ModuleA(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool LoadOnDemand => false;
    }
}
