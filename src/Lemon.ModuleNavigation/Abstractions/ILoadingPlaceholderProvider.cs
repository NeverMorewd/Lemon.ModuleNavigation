namespace Lemon.ModuleNavigation.Abstractions;

public interface ILoadingPlaceholderProvider<T>
{
    T CreateLoadingIndicator(NavigationContext context);
}
