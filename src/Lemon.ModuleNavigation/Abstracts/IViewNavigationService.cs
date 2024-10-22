namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IViewNavigationService
    {
        IDisposable BindingViewNavigationHandler(IViewNavigationHandler handler);
        void NavigateToView(string containerKey, string viewKey);
        void NavigateToView<TView>(string containerKey) where TView : IView;
    }
}
