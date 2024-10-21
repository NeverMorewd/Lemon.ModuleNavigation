namespace Lemon.ModuleNavigation.Abstracts
{
    public interface INavigationHandler
    {
        void OnNavigateTo(string moduleKey);
        IEnumerable<IModule> Modules { get; }
        IModule? CurrentModule { get; }
    }
}
