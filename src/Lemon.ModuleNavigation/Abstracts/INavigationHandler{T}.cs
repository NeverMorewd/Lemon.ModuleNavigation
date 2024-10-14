namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationHandler<in T> :INavigationHandler where T : IModule
    {
        void OnNavigateTo(T module);
    }
}
