namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationDialogService<T> : INavigationDialogService where T : IModule
    {
        IDisposable OnNavigationDialog(INavigationDialogHandler<T> handler);
        void NavigateToDialog(T module);
    }
}
