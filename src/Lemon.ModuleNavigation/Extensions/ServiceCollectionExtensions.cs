using Lemon.ModuleNavigation;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection serviceDescriptors, string viewKey)
        where TView : class, IView
        where TViewModel : class, INavigationAware
    {
        if (!ViewManager.InternalViewDiscriptions.TryAdd(key: viewKey, 
            new ViewDiscription
            {
               ViewKey = viewKey,
               ViewClassName = typeof(TView).Name,
               ViewModelType = typeof(TViewModel),
               ViewType = typeof(TView)
            }))
        {
            throw new InvalidOperationException($"Duplicated key is not allowed:{viewKey}!");
        }

        serviceDescriptors
            .AddTransient<TViewModel>()
            .AddTransient<TView>()
            .AddKeyedTransient<IView>(viewKey, (sp, key) =>
            {
                var view = sp.GetRequiredService<TView>();
                return view;
            })
            .AddKeyedTransient<INavigationAware>(viewKey, (sp, key) =>
            {
                var viewModel = sp.GetRequiredService<TViewModel>();
                return viewModel;
            });
        if (typeof(TViewModel).IsAssignableTo(typeof(IDialogAware)))
        {
            serviceDescriptors.AddKeyedTransient(viewKey, (sp, key) =>
            {
                var viewModel = sp.GetRequiredService<TViewModel>();
                return (IDialogAware)viewModel;
            });
        }
        return serviceDescriptors;
    }
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
                    return (viewModel as IModuleNavigationAware)!;
                }
                else
                {
                    var viewModel = ActivatorUtilities.CreateInstance(sp, module.ViewModelType, moduleSp);
                    return (viewModel as IModuleNavigationAware)!;
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
            .AddSingleton<IModuleNavigationService<IModule>>(sp => sp.GetRequiredService<NavigationService>())
            .AddSingleton<INavigationService>(sp => sp.GetRequiredService<NavigationService>())
            .AddSingleton<INavigationHandler, NavigationHandler>()
            .AddSingleton<IModuleManager, ModuleManager>()
            .AddSingleton<IRegionManager, RegionManager>();
    }

    #region Async implementation
    private static IServiceCollection AddAsyncViewNavigation(this IServiceCollection services)
    {
        services.TryAddSingleton<AsyncViewNavigationService>();
        services.TryAddSingleton<IAsyncViewNavigationService>(sp => sp.GetRequiredService<AsyncViewNavigationService>());
        services.TryAddSingleton<IAsyncViewNavigationHandler, AsyncViewNavigationHandler>();
        services.AddSingleton<IAsyncRegionManager, AsyncRegionManager>();
        return services;
    }

    public static IServiceCollection AddAsyncView<TView, TAsyncViewModel>(this IServiceCollection serviceDescriptors, string viewKey)
        where TView : class, IView
        where TAsyncViewModel : class, IAsyncNavigationAware
    {
        if (!ViewManager.InternalViewDiscriptions.TryAdd(key: viewKey,
            new ViewDiscription
            {
                ViewKey = viewKey,
                ViewClassName = typeof(TView).Name,
                ViewModelType = typeof(TAsyncViewModel),
                ViewType = typeof(TView)
            }))
        {
            throw new InvalidOperationException($"Duplicated key is not allowed:{viewKey}!");
        }

        serviceDescriptors
            .AddTransient<TAsyncViewModel>()
            .AddTransient<TView>()
            .AddKeyedTransient<IView>(viewKey, (sp, key) =>
            {
                var view = sp.GetRequiredService<TView>();
                return view;
            })
            .AddKeyedTransient<IAsyncNavigationAware>(viewKey, (sp, key) =>
            {
                var viewModel = sp.GetRequiredService<TAsyncViewModel>();
                return viewModel;
            });
        if (typeof(TAsyncViewModel).IsAssignableTo(typeof(IDialogAware)))
        {
            serviceDescriptors.AddKeyedTransient(viewKey, (sp, key) =>
            {
                var viewModel = sp.GetRequiredService<TAsyncViewModel>();
                return (IDialogAware)viewModel;
            });
        }
        return serviceDescriptors;
    }

    public static IServiceCollection AddAsyncNavigationSupport(this IServiceCollection serviceDescriptors)
    {
        return AddAsyncViewNavigation(serviceDescriptors);
    }
    #endregion

    public static IServiceCollection AddAppServiceProvider(this IServiceCollection serviceDescriptors,
        IServiceProvider serviceProvider)
    {
        return serviceDescriptors.AddSingleton<IServiceProviderDecorator>(_ => new ServiceProviderDecorator(serviceProvider));
    }
}
