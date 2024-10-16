using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.AdvancedDI;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.ModuleNavigation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule>(this IServiceCollection serviceDescriptors) where TModule : class, IModule
        {
            serviceDescriptors = serviceDescriptors
                .AddTransient<TModule>()
                .AddKeyedTransient<IModule>(typeof(TModule).Name, (sp, key) => sp.GetRequiredService<TModule>())
                .AddKeyedTransient<IModule, TModule>(nameof(IModule), (sp, key) => sp.GetRequiredService<TModule>())
                .AddKeyedTransient(typeof(TModule).Name, (sp, key) =>
                {
                    var module = sp.GetRequiredService<TModule>();
                    var view = ActivatorUtilities.CreateInstance(sp, module.ViewType);
                    return (view as IView)!;
                })
                .AddKeyedTransient(typeof(TModule).Name, (sp, key) =>
                {
                    var module = sp.GetRequiredService<TModule>();
                    var asp = sp.GetKeyedService<IAdvancedServiceProvider>(typeof(TModule).Name);
                    if (asp == null)
                    {
                        var viewModel = ActivatorUtilities.CreateInstance(sp, module.ViewModelType);
                        return (viewModel as IViewModel)!;
                    }
                    else
                    {
                        var viewModel = ActivatorUtilities.CreateInstance(sp, module.ViewModelType, asp);
                        return (viewModel as IViewModel)!;
                    }
                })
                .AddKeyedTransient(typeof(TModule).Name, (sp, key) =>
                 {
                     var module = sp.GetRequiredService<TModule>();
                     if (module is ISelfHostModule selfHostModule)
                     {
                         return new AdvancedServiceProvider(selfHostModule.SelfServiceCollection) as IAdvancedServiceProvider;
                     }
                     return default!;
                 });
            return serviceDescriptors;
        }

        public static IServiceCollection AddModulesBuilder(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors = serviceDescriptors.AddSingleton(sp => sp.GetKeyedServices<IModule>(nameof(IModule)));
            return serviceDescriptors;
        }

        public static IServiceCollection AddNavigationContext(this IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors
                .AddModulesBuilder()
                .AddSingleton<NavigationService>()
                .AddSingleton<INavigationService<IModule>>(sp => sp.GetRequiredService<NavigationService>())
                .AddSingleton<NavigationContext>();
        }
    }
}
