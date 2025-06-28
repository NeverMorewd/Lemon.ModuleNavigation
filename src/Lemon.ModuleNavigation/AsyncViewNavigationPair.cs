using Lemon.ModuleNavigation.Abstractions;

namespace Lemon.ModuleNavigation;

public sealed record AsyncViewNavigationPair(IView View, IAsyncNavigationAware NavigationAware)
{
    public async Task<bool> IsNavigationTargetAsync(NavigationContext context)
        => await NavigationAware.IsNavigationTargetAsync(context);

    public async Task OnNavigatedFromAsync(NavigationContext context)
        => await NavigationAware.OnNavigatedFromAsync(context);

    public async Task OnNavigatedToAsync(NavigationContext context)
    {
        await NavigationAware.OnNavigatedToAsync(context);
    }
}
