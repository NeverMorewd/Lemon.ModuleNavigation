namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IViewNavigationService
    {
        void NavigateToView(string viewKey);
        void NavigateToView<TView>();
    }
}
