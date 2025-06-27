namespace Lemon.ModuleNavigation.Core;

public class NavigationEventArgs : EventArgs
{
    public NavigationContext Context { get; }

    public NavigationEventArgs(NavigationContext context)
    {
        Context = context;
    }
}
