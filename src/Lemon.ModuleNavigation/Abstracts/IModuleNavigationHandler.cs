namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IModuleNavigationHandler
    {
        void OnNavigateTo(string moduleKey);
        IEnumerable<IModule> Modules { get; }
        IModule? CurrentModule { get; }
    }
}
