using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface ISelfHostModule
    {
        IServiceCollection SelfServiceCollection { get; }
    }
}
