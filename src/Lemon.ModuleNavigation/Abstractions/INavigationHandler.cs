namespace Lemon.ModuleNavigation.Abstractions;

public interface INavigationHandler : IModuleNavigationHandler<IModule>, IViewNavigationHandler
{
    IRegionManager RegionManager { get; }
    IModuleManager ModuleManager { get; }
}
