using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IRegionManager: IObservable<NavigationContext>, IObservable<IRegion>
{
    void AddRegion(string regionName, IRegion region);
    IRegion? GetRegion(string regionName);
    void RequestNavigate(string regionName, string viewName, NavigationParameters? parameters = null);
    [Obsolete("requestNew was obsolete.Consider IsNavigationTarget() in INavigationAware instead.")]
    void RequestNavigate(string regionName, string viewName, bool requestNew, NavigationParameters? parameters = null);
    void RequestUnload(string regionName, string viewName);
    void RequestUnload(NavigationContext context);
}
