using Avalonia;
using Lemon.Toolkit.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Lemon.Toolkit.Framework
{
    public static class Extensions
    {
        //[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public static IServiceCollection AddTabModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule>(this IServiceCollection serviceDescriptors) where TModule : class, ITabModule
        {
            serviceDescriptors = serviceDescriptors
                .AddSingleton<TModule>()
                .AddKeyedSingleton<ITabModule, TModule>(nameof(ITabModule), (sp, key) => sp.GetRequiredService<TModule>())
                .AddKeyedSingleton<IView>(typeof(TModule).Name, (sp, key) =>
                {
                    var module = sp.GetRequiredService<TModule>();
                    var view = ActivatorUtilities.CreateInstance(sp, module.ViewType);
                    return (view as IView)!;
                })
                .AddKeyedSingleton<IViewModel>(typeof(TModule).Name, (sp, key) =>
                {
                    var module = sp.GetRequiredService<TModule>();
                    var viewModel = ActivatorUtilities.CreateInstance(sp, module.ViewModelType);
                    return (viewModel as IViewModel)!;
                });
            return serviceDescriptors;
        }

        public static IServiceCollection AddTabModulesBuilder(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors = serviceDescriptors.AddSingleton(sp => sp.GetKeyedServices<ITabModule>(nameof(ITabModule)));
            return serviceDescriptors;
        }
    }
}
