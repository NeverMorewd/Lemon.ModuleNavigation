using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationAware
    {
        void OnNavigatedTo(NavigationContext navigationContext);
        bool IsNavigationTarget(NavigationContext navigationContext);
        void OnNavigatedFrom(NavigationContext navigationContext);
    }
}
