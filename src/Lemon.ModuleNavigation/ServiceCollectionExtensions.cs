using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Internals;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Lemon.ModuleNavigation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule>(this IServiceCollection serviceDescriptors)
            where TModule : class, IModule
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
                    var moduleSp = sp.GetKeyedService<IModuleServiceProvider>(typeof(TModule).Name);
                    if (moduleSp == null)
                    {
                        var viewModel = ActivatorUtilities.CreateInstance(sp, module.ViewModelType);
                        return (viewModel as IViewModel)!;
                    }
                    else
                    {
                        var viewModel = ActivatorUtilities.CreateInstance(sp, module.ViewModelType, moduleSp);
                        return (viewModel as IViewModel)!;
                    }
                })
                .AddKeyedTransient(typeof(TModule).Name, (sp, key) =>
                 {
                     var module = sp.GetRequiredService<TModule>();
                     if (module is IModuleScope moduleScope)
                     {
                         return new ModuleServiceProvider(moduleScope.ScopeServiceProvider!) as IModuleServiceProvider;
                     }
                     return default!;
                 });
            return serviceDescriptors;
        }

        private static IServiceCollection AddModulesBuilder(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors = serviceDescriptors.AddSingleton(sp => sp.GetKeyedServices<IModule>(nameof(IModule)));
            return serviceDescriptors;
        }

        public static IServiceCollection AddNavigationSupport(this IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors
                .AddModulesBuilder()
                .AddSingleton<NavigationService>()
                .AddSingleton<INavigationService<IModule>>(sp => sp.GetRequiredService<NavigationService>())
                .AddSingleton<INavigationContext,NavigationContext>();
        }

        public static IServiceCollection AddAppServiceProvider(this IServiceCollection serviceDescriptors,
            IServiceProvider serviceProvider)
        {
            return serviceDescriptors.AddSingleton<IServiceProviderDecorator>(_ => new ServiceProviderDecorator(serviceProvider));
        }

        public static IServiceCollection RegisterView<TView, TViewModel>(this IServiceCollection serviceDescriptors, 
            string containerName) 
            where TView : class, IView 
            where TViewModel : class, IViewModel
        {
            return
            serviceDescriptors
                .AddTransient<TViewModel>()
                .AddKeyedTransient<TView>(containerName, (sp, key) =>
                {
                    var viewModel = sp.GetRequiredService<TViewModel>();
                    var view = ActivatorUtilities.CreateInstance<TView>(sp);
                    view.SetDataContext(viewModel);
                    return view;
                });
        }
        public static IServiceCollection RegisterView<TView>(this IServiceCollection serviceDescriptors, 
            string containerName, 
            Func<IServiceProvider, IViewModel> viewModelBuilder) 
            where TView : class, IView
        {
            return
            serviceDescriptors
                .AddKeyedTransient<TView>(containerName, (sp, key) =>
                {
                    var viewModel = viewModelBuilder.Invoke(sp);
                    var view = ActivatorUtilities.CreateInstance<TView>(sp);
                    view.SetDataContext(viewModel);
                    return view;
                });
        }
    }
}
