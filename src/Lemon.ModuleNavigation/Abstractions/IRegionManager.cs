using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IRegionManager : IObservable<NavigationContext>, IObservable<IRegion>
{
    void AddRegion(string regionName, IRegion region);
    IRegion? GetRegion(string regionName);
    void RequestViewNavigate(string regionName, string viewName, NavigationParameters? parameters = null, string? alias = null);
    [Obsolete("requestNew was obsolete.Consider IsNavigationTarget() in INavigationAware instead.")]
    void RequestNavigate(string regionName, string viewName, bool requestNew, NavigationParameters? parameters = null);
    void RequestViewUnload(string regionName, string viewName);
    void RequestViewUnload(NavigationContext context);
    void RequestModuleNavigate(string regionName, string moduleName, NavigationParameters? parameters);
    void RequestModuleNavigate(string regionName, IModule module, NavigationParameters? parameters);
    void RequestModuleUnload(string moduleName, string viewName);
    void RequestModuleUnload(IModule module);
}
