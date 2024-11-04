using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationHandler : IModuleNavigationHandler<IModule>, IViewNavigationHandler
    {
        IRegionManager RegionManager { get; }
        IModuleManager ModuleManager { get; }
    }
}
