using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IModuleManager
    {
        ObservableCollection<IModule> ActiveModules
        {
            get;
            set;
        }
    }
}
