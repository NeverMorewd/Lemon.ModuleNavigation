
using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationContainerManager
    {
        void AddContainer(string containerName, INavigationContainer container);
        INavigationContainer? GetContainer(string containerName);
        void RequestNavigate(string containerName, string viewName, bool requestNae, NavigationParameters? parameters = null);
    }
}
