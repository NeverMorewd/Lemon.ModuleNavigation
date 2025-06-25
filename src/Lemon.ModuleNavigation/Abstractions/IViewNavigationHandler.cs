using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IViewNavigationHandler
{
    void OnNavigateTo(string regionName, 
        string viewName);
    void OnNavigateTo(string regionName, 
        string viewName,
        NavigationParameters parameters);
    void OnViewUnload(string regionName,
        string viewName);
}
