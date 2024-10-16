using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IModuleScope
    {
        IServiceCollection ScopeServiceCollection { get; }
        IServiceProvider ScopeServiceProvider { get; }
    }
}
