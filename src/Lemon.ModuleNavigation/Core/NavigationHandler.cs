using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Core
{
    public class NavigationHandler : INavigationHandler, IDisposable
    {
        private readonly IDisposable _cleanup1;
        private readonly IDisposable _cleanup2;
        private readonly INavigationService _navigationService;
        private readonly IRegionManager _containerManager;
        private readonly IModuleManager _moduleManager;
        public NavigationHandler(INavigationService navigationService,
            IRegionManager containerManager,
            IModuleManager moduleManager)
        {
            _navigationService = navigationService;
            _containerManager = containerManager;
            _moduleManager = moduleManager;
            _cleanup1 = _navigationService.BindingNavigationHandler(this);
            _cleanup2 = _navigationService.BindingViewNavigationHandler(this);
        }
        public IRegionManager ContainerManager => _containerManager;
        public IModuleManager ModuleManager => _moduleManager;


        public void OnNavigateTo(IModule module, NavigationParameters parameter)
        {
            _moduleManager.RequestNavigate(module, parameter);
        }
        public void OnNavigateTo(string moduleKey, NavigationParameters parameters)
        {
            _moduleManager.RequestNavigate(moduleKey, parameters);
        }
        public void OnNavigateTo(string containerName,
             string viewName,
             bool requestNew = false)
        {
            ContainerManager.RequestNavigate(containerName, viewName, requestNew, null);
        }
        public void OnNavigateTo(string containerName,
            string viewName,
            NavigationParameters navigationParameters,
            bool requestNew = false)
        {
            ContainerManager.RequestNavigate(containerName, viewName, requestNew, navigationParameters);
        }

        void IDisposable.Dispose()
        {
            _cleanup1?.Dispose();
            _cleanup2?.Dispose();
        }
    }
}
