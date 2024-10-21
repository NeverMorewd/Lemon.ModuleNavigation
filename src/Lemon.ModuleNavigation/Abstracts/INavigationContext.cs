namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationContext : INavigationHandler<IModule>
    {
        IServiceProvider ServiceProvider { get; }
        IView CreateNewView(IModule module);
    }
}
