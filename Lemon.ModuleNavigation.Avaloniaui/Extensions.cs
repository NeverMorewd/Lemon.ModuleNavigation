using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Avaloniaui.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public static class Extensions
    {
        public static IServiceCollection AddNavigationContext(this IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors
                .AddModulesBuilder()
                .AddSingleton<NavigationService>()
                .AddSingleton<INavigationService<IModule>>(sp=>sp.GetRequiredService<NavigationService>())
                .AddSingleton<NavigationContext>();
        }
    }
}
