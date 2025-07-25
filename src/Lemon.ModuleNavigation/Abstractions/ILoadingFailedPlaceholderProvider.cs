namespace Lemon.ModuleNavigation.Abstractions;

public interface ILoadingFailedPlaceholderProvider<T>
{
    T CreateErrorIndicator(NavigationContext context, string errorMessage);
}
