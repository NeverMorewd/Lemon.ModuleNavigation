using Lemon.ModuleNavigation.Abstractions;

namespace Lemon.ModuleNavigation;

public sealed record ViewNavigationPair(IView View, INavigationAware NavigationAware)
{
    public bool IsNavigationTargetAsync(NavigationContext context)
        => NavigationAware.IsNavigationTarget(context);

    public void OnNavigatedFrom(NavigationContext context)
        => NavigationAware.OnNavigatedFrom(context);

    public void OnNavigatedTo(NavigationContext context)
    {
        View.DataContext = NavigationAware;
        NavigationAware.OnNavigatedTo(context);
        context.Alias = NavigationAware.Alias;
    }
}