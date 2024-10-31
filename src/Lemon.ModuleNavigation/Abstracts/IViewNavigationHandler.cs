using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IViewNavigationHandler
    {
        void OnNavigateTo(string containerName, 
            string viewName, 
            bool requestNew);
        void OnNavigateTo(string containerName, 
            string viewName,
            NavigationParameters parameters,
            bool requestNew);
    }
}
