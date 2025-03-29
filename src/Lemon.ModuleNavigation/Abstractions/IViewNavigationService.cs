using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IViewNavigationService
{
    IDisposable BindingViewNavigationHandler(IViewNavigationHandler handler);
    void RequestViewNavigation(string regionName, 
        string viewName, 
        bool requestNew = false);
    void RequestViewNavigation(string regionName,
        string viewName,
        NavigationParameters parameters,
        bool requestNew = false);
    void RequestViewUnload(string regionName,
       string viewName);
}
