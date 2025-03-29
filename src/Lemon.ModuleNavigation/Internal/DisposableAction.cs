namespace Lemon.ModuleNavigation.Internal;

internal sealed class DisposableAction : IDisposable
{
    private readonly Action _action;
    private int _disposed;

    public DisposableAction(Action action)
    {
        _action = action;
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 0)
        {
            _action();
        }
    }
}
