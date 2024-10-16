using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.ModuleNavigation.Sample.ModuleCs.ViewModels;
using Lemon.ModuleNavigation.Sample.ModuleCs.Views;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleCs.SubModules
{
    public class SubModule02 : AvaModule<SubView02, SubViewModel02>
    {
        public SubModule02(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public override bool LoadOnDemand => false;
    }
}
