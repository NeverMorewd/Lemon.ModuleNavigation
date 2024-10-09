namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationService<out T>
    {
        IDisposable OnNavigation(INavigationHandler<T> navigation);
    }
}
