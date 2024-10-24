namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationService
    {
        IDisposable BindingNavigationHandler(INavigationHandler handler);
        void NavigateTo(string moduleKey);
    }
}
