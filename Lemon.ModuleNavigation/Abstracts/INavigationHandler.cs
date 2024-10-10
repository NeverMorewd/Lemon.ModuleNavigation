namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationHandler<in T>
    {
        void OnNavigateTo(T target);
    }
}
