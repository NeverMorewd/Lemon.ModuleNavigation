using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions
{
    public interface IModuleNavigationService
    {
        IDisposable BindingNavigationHandler(IModuleNavigationHandler handler);
        void RequestModuleNavigate(string moduleKey, NavigationParameters parameters);
    }
}
