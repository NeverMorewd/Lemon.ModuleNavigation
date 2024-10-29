using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationHandler : IModuleNavigationHandler<IModule>, IViewNavigationHandler
    {
        IServiceProvider ServiceProvider { get; }
        ObservableCollection<IModule> ActiveModules { get; set; }
        IView CreateNewView(IModule module);
    }
}
