namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IModuleNavigationService<T> : IModuleNavigationService where T : IModule
    {
        IDisposable BindingNavigationHandler(IModuleNavigationHandler<T> handler);
        void NavigateTo(T module);
    }
}
