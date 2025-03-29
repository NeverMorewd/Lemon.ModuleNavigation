using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IViewNavigationHandler
{
    void OnNavigateTo(string regionName, 
        string viewName, 
        bool requestNew);
    void OnNavigateTo(string regionName, 
        string viewName,
        NavigationParameters parameters,
        bool requestNew);
    void OnViewUnload(string regionName,
        string viewName);
}
