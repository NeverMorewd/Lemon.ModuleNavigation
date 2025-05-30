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
    /// </param>
    [Obsolete("requestNew was obsolete.Consider IsNavigationTarget() in INavigationAware instead.")]
    void RequestViewNavigation(string regionName, 
        string viewName,
        bool requestNew);

    void RequestViewNavigation(string regionName,
        string viewName, 
        string? alias = null);

    /// <summary>
    /// RequestViewNavigation
    /// </summary>
    /// <param name="regionName"></param>
    /// <param name="viewName"></param>
    /// <param name="parameters"></param>
    /// <param name="requestNew">
    /// requestNew can not decide wether to get a new instance of View,
    /// It is controlled by IsNavigationTarget() in INavigationAware.
    /// </param>
    [Obsolete("requestNew was obsolete.Consider IsNavigationTarget() in INavigationAware instead.")]
    void RequestViewNavigation(string regionName,
        string viewName,
        NavigationParameters parameters,
        bool requestNew);

    void RequestViewNavigation(string regionName,
        string viewName,
        NavigationParameters parameters,
        string? alias = null);

    /// <summary>
    /// RequestViewUnload
    /// </summary>
    /// <param name="regionName"></param>
    /// <param name="viewName"></param>
    void RequestViewUnload(string regionName,
       string viewName);
}
