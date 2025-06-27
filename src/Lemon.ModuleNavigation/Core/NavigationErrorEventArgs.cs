namespace Lemon.ModuleNavigation.Core;

public class NavigationErrorEventArgs : EventArgs
{
    public NavigationContext Context { get; }
    public Exception Exception { get; }

    public NavigationErrorEventArgs(NavigationContext context, Exception exception)
    {
        Context = context;
        Exception = exception;
    }
}
