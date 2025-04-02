using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IModuleNavigationService<T> : IModuleNavigationService where T : IModule
{
    IDisposable BindingNavigationHandler(IModuleNavigationHandler<T> handler);
    void RequestModuleNavigate(T module, NavigationParameters? parameters);
}
