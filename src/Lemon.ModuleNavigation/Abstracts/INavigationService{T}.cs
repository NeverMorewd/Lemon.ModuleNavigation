namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationService<out T> :INavigationService where T : IModule
    {
        IDisposable OnNavigation(INavigationHandler<T> handler);
    }
}
