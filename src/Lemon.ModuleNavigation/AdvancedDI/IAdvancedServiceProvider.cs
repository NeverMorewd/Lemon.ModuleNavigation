using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.AdvancedDI
{
    /// <summary>
    /// Service provider that allows for dynamic adding of new services
    /// </summary>
    public interface IAdvancedServiceProvider : IServiceProvider
    {
        /// <summary>
        /// Add services to this collection
        /// </summary>
        IServiceCollection ServiceCollection { get; }
    }
}
