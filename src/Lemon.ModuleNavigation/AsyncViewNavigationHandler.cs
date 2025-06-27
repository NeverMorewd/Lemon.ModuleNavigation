using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation;

public sealed class AsyncViewNavigationHandler : IAsyncViewNavigationHandler
{
    private readonly ValueTask<IDisposable> _cleanupTask;
    private readonly IAsyncViewNavigationService _navigationService;
    private readonly IAsyncRegionManager _regionManager;
    public AsyncViewNavigationHandler(IAsyncViewNavigationService navigationService,
        IAsyncRegionManager regionManager)
    {
        _navigationService = navigationService;
        _regionManager = regionManager;
        _cleanupTask = _navigationService.RegisterNavigationHandlerAsync(this);
    }
    public IAsyncRegionManager AsyncRegionManager => _regionManager;

    public async ValueTask DisposeAsync()
    {
        (await _cleanupTask.ConfigureAwait(false)).Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task OnNavigateToAsync(string regionName,
        string viewName,
        NavigationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        await AsyncRegionManager.RequestViewNavigateAsync(regionName,
            viewName,
            parameters,
            cancellationToken);
    }

    public async Task OnViewUnloadAsync(string regionName,
        string viewName,
        CancellationToken cancellationToken = default)
    {
        await AsyncRegionManager.RequestViewUnloadAsync(regionName,
            viewName,
            cancellationToken);
    }
}
