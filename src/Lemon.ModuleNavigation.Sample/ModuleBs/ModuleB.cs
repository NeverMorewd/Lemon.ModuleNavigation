using System;

namespace Lemon.ModuleNavigation.Sample.ModuleBs;

public class ModuleB : Module<ViewB, ViewModelB>
{
    public ModuleB(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override bool LoadOnDemand => true;
}
