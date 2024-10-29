namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationService
    {
        IDisposable BindingNavigationHandler(IModuleNavigationHandler handler);
        void NavigateTo(string moduleKey);
    }
}
