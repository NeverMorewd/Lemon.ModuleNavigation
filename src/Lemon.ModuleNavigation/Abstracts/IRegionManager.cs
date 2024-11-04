
using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IRegionManager
    {
        void AddContainer(string containerName, IRegion container);
        IRegion? GetContainer(string containerName);
        void RequestNavigate(string containerName, string viewName, bool requestNae, NavigationParameters? parameters = null);
    }
}
