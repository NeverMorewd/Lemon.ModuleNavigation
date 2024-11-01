using Lemon.ModuleNavigation.Core;
using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IModuleManager
    {
        IModule? CurrentModule { get; }
        IEnumerable<IModule> Modules
        {
            get;
        }
        ObservableCollection<IModule> ActiveModules
        {
            get;
        }
        IView CreateView(IModule module);
        void RequestNavigate(string moduleName, NavigationParameters parameters);
        void RequestNavigate(IModule module, NavigationParameters parameters);
    }
}
