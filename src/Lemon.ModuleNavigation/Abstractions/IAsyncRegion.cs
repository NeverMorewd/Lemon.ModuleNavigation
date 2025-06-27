namespace Lemon.ModuleNavigation.Abstractions;

public interface IAsyncRegion
{
    string Name { get; }
    Task ActivateAsync(NavigationContext target);
    Task DeActivateAsync(NavigationContext target);
}
