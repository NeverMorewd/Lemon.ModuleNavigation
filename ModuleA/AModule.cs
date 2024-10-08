using Lemon.Extensions.SlimModule;
using ModuleA.ViewModels;

namespace ModuleA
{
    public class AModule : Module<ViewA, ViewModelA>
    {
        public AModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override bool LoadOnDemand => true;
    }
}
