using Lemon.ModuleNavigation.Avaloniaui.Core;
using Lemon.ModuleNavigation.Sample.ModuleCs.ViewModels;
using Lemon.ModuleNavigation.Sample.ModuleCs.Views;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleCs.SubModules
{
    public class SubModule01 : AvaModule<SubView01, SubViewModel01>
    {
        public SubModule01(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public override bool LoadOnDemand => false;
    }
}
