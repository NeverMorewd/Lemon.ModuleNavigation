namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationContextProvider
    {
        INavigationHandler NavigationContext { get; }
    }
}
