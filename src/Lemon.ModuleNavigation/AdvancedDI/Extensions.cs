using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.AdvancedDI
{
    public static class Extensions
    {
        public static AdvancedServiceProvider BuildAdvancedServiceProvider(this IServiceCollection serviceDescriptors)
        {
            return new AdvancedServiceProvider(serviceDescriptors);
        }
    }
}
