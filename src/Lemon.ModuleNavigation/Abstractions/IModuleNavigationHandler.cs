using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions
{
    public interface IModuleNavigationHandler
    {
        void OnNavigateTo(string moduleKey, NavigationParameters parameters);
    }
}
