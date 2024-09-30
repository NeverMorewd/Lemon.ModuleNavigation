using Lemon.Hosting.Modularization.Abstracts;
using Lemon.Hosting.Modularization;
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
