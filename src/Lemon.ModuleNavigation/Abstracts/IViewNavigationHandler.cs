namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IViewNavigationHandler
    {
        void OnNavigateTo(string containerName, string viewName);
        void OnNavigateTo<TView>(string containerName) where TView : IView;
    }
}
