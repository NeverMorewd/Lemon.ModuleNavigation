namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IModuleNavigationService
    {
        IDisposable BindingNavigationHandler(IModuleNavigationHandler handler);
        void NavigateTo(string moduleKey);
    }
}
