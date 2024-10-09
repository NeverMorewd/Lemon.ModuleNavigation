namespace Lemon.Extensions.ModuleNavigation.Abstracts
{
    public interface INavigationHandler<in T>
    {
        void NavigateTo(T target);
    }
}
