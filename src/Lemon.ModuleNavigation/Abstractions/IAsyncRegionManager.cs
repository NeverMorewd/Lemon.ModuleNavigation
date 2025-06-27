using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IAsyncRegionManager
{
    void AddRegion(string regionName, IAsyncRegion region);
    IAsyncRegion? GetRegion(string regionName);
    Task RequestViewNavigateAsync(string regionName, string viewName, NavigationParameters? parameters = null, CancellationToken cancellationToken = default);
    Task RequestViewUnloadAsync(string regionName, string viewName, CancellationToken cancellationToken = default);
    Task RequestViewUnloadAsync(NavigationContext context, CancellationToken cancellationToken = default);
}
