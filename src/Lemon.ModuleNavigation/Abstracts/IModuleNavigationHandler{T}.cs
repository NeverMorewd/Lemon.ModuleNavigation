using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IModuleNavigationHandler<in T> :IModuleNavigationHandler where T : IModule
    {
        void OnNavigateTo(T module, NavigationParameters parameter);
    }
}
