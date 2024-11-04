using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationHandler : IModuleNavigationHandler<IModule>, IViewNavigationHandler
    {
        IRegionManager ContainerManager { get; }
        IModuleManager ModuleManager { get; }
    }
}
