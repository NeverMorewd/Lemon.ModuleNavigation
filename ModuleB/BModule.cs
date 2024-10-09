using Lemon.Extensions.ModuleNavigation.Abstracts;
using Lemon.Extensions.ModuleNavigation;
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
