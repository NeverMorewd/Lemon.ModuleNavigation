using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IAsyncViewNavigationHandler : IAsyncDisposable
{
    IAsyncRegionManager AsyncRegionManager { get; }
    Task OnNavigateToAsync(string regionName,
        string viewName,
        NavigationParameters? parameters = null,
        CancellationToken cancellationToken = default);
    Task OnViewUnloadAsync(string regionName,
        string viewName,
        CancellationToken cancellationToken = default);
}