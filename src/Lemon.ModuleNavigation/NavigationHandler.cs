using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation;

public class NavigationHandler : INavigationHandler, IDisposable
{
    private readonly IDisposable _cleanup1;
    private readonly IDisposable _cleanup2;
    private readonly INavigationService _navigationService;
    private readonly IRegionManager _regionManager;
    private readonly IModuleManager _moduleManager;
    public NavigationHandler(INavigationService navigationService,
        IRegionManager regionManager,
        IModuleManager moduleManager)
    {
        _navigationService = navigationService;
        _regionManager = regionManager;
        _moduleManager = moduleManager;
        _cleanup1 = _navigationService.BindingNavigationHandler(this);
        _cleanup2 = _navigationService.BindingViewNavigationHandler(this);
    }
    public IRegionManager RegionManager => _regionManager;
    public IModuleManager ModuleManager => _moduleManager;


    public void OnNavigateTo(IModule module, NavigationParameters? parameter)
    {
        _moduleManager.RequestNavigate(module, parameter);
    }
    public void OnNavigateTo(string moduleKey, NavigationParameters? parameters)
    {
        _moduleManager.RequestNavigate(moduleKey, parameters);
    }
    public void OnNavigateTo(string regionName,
         string viewName)
    {
        RegionManager.RequestViewNavigate(regionName, viewName, null);
    }
    public void OnNavigateTo(string regionName,
        string viewName,
        NavigationParameters navigationParameters)
    {
        RegionManager.RequestViewNavigate(regionName, viewName, navigationParameters);
    }

    public void OnViewUnload(string regionName, string viewName)
    {
        RegionManager.RequestViewUnload(regionName, viewName);
    }

    void IDisposable.Dispose()
    {
        _cleanup1?.Dispose();
        _cleanup2?.Dispose();
    }
}
