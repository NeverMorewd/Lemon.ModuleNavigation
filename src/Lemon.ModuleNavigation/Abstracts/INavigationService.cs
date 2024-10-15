namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationService
    {
        IDisposable OnNavigation(INavigationHandler handler);
        void NavigateTo(string moduleKey);
    }
}
