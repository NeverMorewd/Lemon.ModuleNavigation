namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationDialogService
    {
        IDisposable OnNavigation(INavigationDialogHandler handler);
        void NavigateToDialog(string moduleKey);
    }
}
