namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationContext : INavigationHandler<IModule>, IViewNavigationHandler
    {
        IServiceProvider ServiceProvider { get; }
        IView CreateNewView(IModule module);
    }
}
