namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationService<T> :INavigationService where T : IModule
    {
        IDisposable BindingNavigationHandler(INavigationHandler<T> handler);
        void NavigateTo(T module);
    }
}
