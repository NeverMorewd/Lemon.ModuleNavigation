namespace Lemon.ModuleNavigation.Abstractions
{
    public interface IAsyncServiceAware
    {
        IServiceProvider ServiceProvider { get; }
    }
}
