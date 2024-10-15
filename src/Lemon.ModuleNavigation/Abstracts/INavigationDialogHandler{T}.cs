namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationDialogHandler<in T> : INavigationDialogHandler where T : IModule
    {
        void OnNavigateToDialog(T module);
    }
}
