using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.ModuleNavigation
{
    public static class Extensions
    {
        public static IServiceCollection AddModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule>(this IServiceCollection serviceDescriptors) where TModule : class, IModule
        {
            serviceDescriptors = serviceDescriptors
                .AddSingleton<TModule>()
                .AddKeyedSingleton<IModule, TModule>(nameof(IModule), (sp, key) => sp.GetRequiredService<TModule>())
                .AddKeyedSingleton(typeof(TModule).Name, (sp, key) =>
                {
                    var module = sp.GetRequiredService<TModule>();
                    var view = ActivatorUtilities.CreateInstance(sp, module.ViewType);
                    return (view as IView)!;
                })
                .AddKeyedSingleton(typeof(TModule).Name, (sp, key) =>
                {
                    var module = sp.GetRequiredService<TModule>();
                    var viewModel = ActivatorUtilities.CreateInstance(sp, module.ViewModelType);
                    return (viewModel as IViewModel)!;
                });
            return serviceDescriptors;
        }

        public static IServiceCollection AddModulesBuilder(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors = serviceDescriptors.AddSingleton(sp => sp.GetKeyedServices<IModule>(nameof(IModule)));
            return serviceDescriptors;
        }
    }
}
