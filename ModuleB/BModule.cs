using Lemon.Extensions.SlimModule.Abstracts;
using Lemon.Extensions.SlimModule;
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
