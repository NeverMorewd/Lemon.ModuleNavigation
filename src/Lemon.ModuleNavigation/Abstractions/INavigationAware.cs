namespace Lemon.ModuleNavigation.Abstractions;

public interface INavigationAware
{
    string? Alias { get; }
    void OnNavigatedTo(NavigationContext navigationContext);
    bool IsNavigationTarget(NavigationContext navigationContext);
    void OnNavigatedFrom(NavigationContext navigationContext);
}
