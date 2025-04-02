namespace Lemon.ModuleNavigation.Abstractions;

public interface INavigationAware
{
    event Action? RequestUnload;
    void OnNavigatedTo(NavigationContext navigationContext);
    bool IsNavigationTarget(NavigationContext navigationContext);
    void OnNavigatedFrom(NavigationContext navigationContext);
}
