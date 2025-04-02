namespace Lemon.ModuleNavigation.Abstractions;

public interface IServiceAware
{
    IServiceProvider ServiceProvider { get; }
}
