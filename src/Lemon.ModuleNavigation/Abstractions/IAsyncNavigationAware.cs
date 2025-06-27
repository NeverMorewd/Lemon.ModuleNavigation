namespace Lemon.ModuleNavigation.Abstractions;

public interface IAsyncNavigationAware
{
    string? Alias { get; }
    Task OnNavigatedToAsync(NavigationContext context);
    Task OnNavigatedFromAsync(NavigationContext context);
    Task<bool> IsNavigationTargetAsync(NavigationContext context);
}
