using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Abstractions
{
    public interface IModuleScope
    {
        IServiceCollection ScopeServiceCollection { get; }
        IServiceProvider ScopeServiceProvider { get; }
    }
}
