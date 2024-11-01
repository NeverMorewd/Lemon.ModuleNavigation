using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationHandler : IModuleNavigationHandler<IModule>, IViewNavigationHandler
    {
        INavigationContainerManager ContainerManager { get; }
        IModuleManager ModuleManager { get; }
    }
}
