using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationContext : INavigationHandler<IModule>, IViewNavigationHandler
    {
        IServiceProvider ServiceProvider { get; }
        ObservableCollection<IModule> ActiveModules { get; set; }
        IView CreateNewView(IModule module);
    }
}
