using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IViewNavigationService
{
    IDisposable BindingViewNavigationHandler(IViewNavigationHandler handler);
    /// <summary>
    /// RequestViewNavigation
    /// </summary>
    /// <param name="regionName"></param>
    /// <param name="viewName"></param>
    /// <param name="requestNew">
    /// requestNew can not decide wether to get a new instance of View,
    /// It is controlled by IsNavigationTarget() in INavigationAware.
    /// We can handle the value of requestNew in IsNavigationTarget() by NavigationContext.
    /// </param>
    void RequestViewNavigation(string regionName, 
        string viewName, 
        bool requestNew = false);

    /// <summary>
    /// RequestViewNavigation
    /// </summary>
    /// <param name="regionName"></param>
    /// <param name="viewName"></param>
    /// <param name="parameters"></param>
    /// <param name="requestNew">
    /// requestNew can not decide wether to get a new instance of View,
    /// It is controlled by IsNavigationTarget() in INavigationAware.
    /// We can handle the value of requestNew in IsNavigationTarget() by NavigationContext.
    /// </param>
    void RequestViewNavigation(string regionName,
        string viewName,
        NavigationParameters parameters,
        bool requestNew = false);

    /// <summary>
    /// RequestViewUnload
    /// </summary>
    /// <param name="regionName"></param>
    /// <param name="viewName"></param>
    void RequestViewUnload(string regionName,
       string viewName);
}
