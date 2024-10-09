using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation;
using ModuleA.ViewModels;

namespace ModuleB
{
    public class BModule : Module<ViewB, ViewModelB>
    {
        public BModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool LoadOnDemand => true;
    }
}
