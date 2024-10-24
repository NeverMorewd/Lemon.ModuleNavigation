namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IViewNavigationHandler
    {
        void OnNavigateTo(string containerName, string viewName, bool requestNew);
    }
}
