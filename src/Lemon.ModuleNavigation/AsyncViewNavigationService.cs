using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Lemon.ModuleNavigation.Internal;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation;

public class AsyncViewNavigationService : IAsyncViewNavigationService, IDisposable
{
    private readonly ConcurrentSet<IAsyncViewNavigationHandler> _viewHandlers = [];
    private readonly ConcurrentQueue<NavigationRequest> _bufferedNavigations = new();
    private readonly SemaphoreSlim _navigationSemaphore = new(1, 1);
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private volatile bool _disposed;

    //public event EventHandler<NavigationErrorEventArgs>? NavigationError;


    public async ValueTask<IDisposable> RegisterNavigationHandlerAsync(
        IAsyncViewNavigationHandler handler,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(handler, nameof(handler));

        if (!_viewHandlers.Add(handler))
        {
            return new DisposableAction(() => { });
        }
        await ProcessBufferedNavigationsForHandlerAsync(handler, cancellationToken)
            .ConfigureAwait(false);

        return new DisposableAction(() =>
        {
            if (!_disposed)
            {
                _viewHandlers.Remove(handler);
            }
        });
    }

    public async Task RequestViewNavigationAsync(
        string regionName,
        string viewName,
        NavigationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        ValidateNavigationParameters(regionName, viewName);

        var navigationRequest = new NavigationRequest(
            NavigationType.Navigate,
            regionName,
            viewName,
            parameters);

        _bufferedNavigations.Enqueue(navigationRequest);

        await ExecuteNavigationAsync(navigationRequest, cancellationToken);
    }

    public async Task RequestViewUnloadAsync(
        string regionName,
        string viewName,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        ValidateNavigationParameters(regionName, viewName);

        var unloadRequest = new NavigationRequest(
            NavigationType.Unload,
            regionName,
            viewName,
            null);

        await ExecuteNavigationAsync(unloadRequest, cancellationToken);
    }

    public void ClearBufferedNavigations()
    {
        ThrowIfDisposed();
        while (_bufferedNavigations.TryDequeue(out _))
        {
            
        }
    }

    public int RegisteredHandlersCount => _viewHandlers.Count;

    private async Task ExecuteNavigationAsync(
        NavigationRequest request,
        CancellationToken cancellationToken)
    {
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
            _cancellationTokenSource.Token,
            cancellationToken);

        await _navigationSemaphore.WaitAsync(combinedCts.Token);

        try
        {
            var handlers = _viewHandlers.ToArray();

            if (handlers.Length == 0)
            {
                return;
            }

            var tasks = handlers.Select(handler =>
                SafeExecuteHandlerAsync(request, handler, combinedCts.Token));

            await Task.WhenAll(tasks);
        }
        finally
        {
            _navigationSemaphore.Release();
        }
    }

    private async Task ProcessBufferedNavigationsForHandlerAsync(
        IAsyncViewNavigationHandler handler,
        CancellationToken cancellationToken)
    {
        var bufferedRequests = _bufferedNavigations.ToArray();

        foreach (var request in bufferedRequests)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            await SafeExecuteHandlerAsync(request, handler, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task SafeExecuteHandlerAsync(
        NavigationRequest request,
        IAsyncViewNavigationHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var task = request.Type switch
            {
                NavigationType.Navigate => handler.OnNavigateToAsync(
                    request.RegionName,
                    request.ViewName,
                    request.Parameters,
                    cancellationToken),
                NavigationType.Unload => handler.OnViewUnloadAsync(
                    request.RegionName,
                    request.ViewName,
                    cancellationToken),
                _ => Task.CompletedTask
            };

            await task;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Operation was cancelled, do nothing
            throw;
        }
    }

    private static void ValidateNavigationParameters(string regionName, string viewName)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNullOrWhiteSpace(regionName, nameof(regionName));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(viewName, nameof(viewName));
#else
        if (string.IsNullOrWhiteSpace(regionName))
            throw new ArgumentException("Region name cannot be null or whitespace", nameof(regionName));
        if (string.IsNullOrWhiteSpace(viewName))
            throw new ArgumentException("View name cannot be null or whitespace", nameof(viewName));
#endif
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(AsyncViewNavigationService));
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        _cancellationTokenSource.Cancel();
        _navigationSemaphore.Dispose();
        _cancellationTokenSource.Dispose();

        _viewHandlers.Clear();
        ClearBufferedNavigations();
    }
}

internal enum NavigationType
{
    Navigate,
    Unload
}

internal record NavigationRequest(
    NavigationType Type,
    string RegionName,
    string ViewName,
    NavigationParameters? Parameters);

//public record NavigationErrorEventArgs(
//    IAsyncViewNavigationHandler Handler,
//    NavigationRequest Request,
//    Exception Exception);

