using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IViewNavigationHandler
{
    void OnNavigateTo(string regionName, 
        string viewName,
        string? alias = null);
    void OnNavigateTo(string regionName, 
        string viewName,
        NavigationParameters parameters,
        string? alias = null);
    void OnViewUnload(string regionName,
        string viewName);
}
