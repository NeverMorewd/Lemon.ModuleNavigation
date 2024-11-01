using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IModuleNavigationHandler
    {
        void OnNavigateTo(string moduleKey, NavigationParameters parameters);
    }
}
