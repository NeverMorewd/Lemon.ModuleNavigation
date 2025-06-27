using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IAsyncViewNavigationService
{
    ValueTask<IDisposable> RegisterNavigationHandlerAsync(IAsyncViewNavigationHandler handler, 
        CancellationToken cancellationToken = default);

    Task RequestViewNavigationAsync(string regionName,
        string viewName,
        NavigationParameters? parameters = null,
        CancellationToken cancellationToken = default);

    Task RequestViewUnloadAsync(string regionName,
       string viewName,
       CancellationToken cancellationToken = default);
}
