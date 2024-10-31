using Lemon.ModuleNavigation.Core;
using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationContainer
    {
        ObservableCollection<NavigationContext> Contexts 
        { 
            get;
        }
        void Activate(NavigationContext target);
        void DeActivate(NavigationContext target);
    }
}
