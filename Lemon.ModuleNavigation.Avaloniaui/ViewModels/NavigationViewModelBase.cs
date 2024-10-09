using Lemon.ModuleNavigation.Avaloniaui.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui.ViewModels
{
    public abstract class NavigationViewModelBase : INavigationContextProvider
    {
        public NavigationViewModelBase(NavigationContext navigationContext) 
        {
            NavigationContext = navigationContext;
        }
        public NavigationContext NavigationContext
        {
            get;
        }
    }
}
