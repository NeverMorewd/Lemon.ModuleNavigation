using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationHandler : IModuleNavigationHandler<IModule>, IViewNavigationHandler
    {
        IServiceProvider ServiceProvider { get; }
        INavigationContainerManager ContainerManager { get; }
        ObservableCollection<IModule> ActiveModules { get; set; }
        IView CreateNewView(IModule module);
    }
}
