using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Lemon.ModuleNavigation.Internals;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.ModuleNavigation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleOld<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule>(this IServiceCollection serviceDescriptors)
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
                });
                //.AddKeyedTransient(typeof(TModule).Name, (sp, key) =>
                // {
                //     var module = sp.GetRequiredService<TModule>();
                //     if (module is IModuleScope moduleScope)
                //     {
                //         return new ModuleServiceProvider(moduleScope.ScopeServiceProvider!) as IModuleServiceProvider;
                //     }
                //     return default!;
                // });
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
                .AddSingleton<INavigationContainerManager, NavigationContainerManager>();
        }

        public static IServiceCollection AddAppServiceProvider(this IServiceCollection serviceDescriptors,
            IServiceProvider serviceProvider)
        {
            return serviceDescriptors.AddSingleton<IServiceProviderDecorator>(_ => new ServiceProviderDecorator(serviceProvider));
        }
        public static IServiceCollection AddModule(IServiceCollection services, Type moduleType)
        {
            var module = ActivatorUtilities.CreateInstance(_serviceProvider, moduleType) as IModule ?? throw new InvalidOperationException($"Failed to create instance of {moduleType.FullName}");
        }
        private static IServiceCollection AddModuleInternal(IServiceCollection services, Type moduleType)
        {
            services.AddSingleton<IModuleFactory>(sp => new ModuleFactory(sp));

            services.AddSingleton(moduleType, (sp) =>
            {
                var factory = sp.GetRequiredService<IModuleFactory>();
                return factory.CreateModule(moduleType);
            });

            // 注册IModule接口
            services.AddSingleton<IModule>(sp => (IModule)sp.GetRequiredService(moduleType));

            // 注册模块描述信息，用于后续延迟注册其他服务
            services.AddSingleton(new ModuleTypeDescriptor(moduleType));

            // 添加服务描述器，它将在服务被请求时处理实际的注册
            services.AddSingleton<IModuleServiceResolver>(sp => new ModuleServiceResolver(sp));

            return services;
        }
    }
    public class ModuleFactory : IModuleFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ModuleFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IModule CreateModule(Type moduleType)
        {
            var module = ActivatorUtilities.CreateInstance(_serviceProvider, moduleType) as IModule ?? throw new InvalidOperationException($"Failed to create instance of {moduleType.FullName}");
            if (!module.LoadOnDemand)
            {
                module.Initialize();
            }
            return module;
        }

        public IModule CreateModule<T>() where T : class, IModule
        {
            var module = ActivatorUtilities.CreateInstance<T>(_serviceProvider) ?? throw new InvalidOperationException($"Failed to create instance of {typeof(T)}");
            if (!module.LoadOnDemand)
            {
                module.Initialize();
            }
            return module;
        }
    }

    public class ModuleTypeDescriptor
    {
        public Type ModuleType { get; }

        public ModuleTypeDescriptor(Type moduleType)
        {
            ModuleType = moduleType;
        }
    }

    public interface IModuleServiceResolver
    {
        object? ResolveService(Type serviceType);
    }

    public class ModuleServiceResolver : IModuleServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, Type?> _implementationCache = new();

        public ModuleServiceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object? ResolveService(Type serviceType)
        {
            // 获取模块实例
            var module = _serviceProvider.GetRequiredService<IModule>();

            // 检查是否是模块提供的服务
            if (!module.ServiceTypes.Contains(serviceType) &&
                !module.ViewTypes.Any(v => v.Key == serviceType || v.Value == serviceType) &&
                serviceType != module.ViewType &&
                serviceType != module.ViewModelType)
            {
                return null;
            }

            // 获取或查找实现类型
            var implementationType = _implementationCache.GetOrAdd(serviceType, type =>
            {
                if (type.IsInterface || type.IsAbstract)
                {
                    return type.Assembly.GetTypes()
                        .FirstOrDefault(t => !t.IsAbstract && !t.IsInterface && type.IsAssignableFrom(t));
                }
                return type;
            });

            if (implementationType == null)
            {
                return null;
            }

            // 创建服务实例
            return ActivatorUtilities.CreateInstance(_serviceProvider, implementationType);
        }
    }

    // 自定义服务提供者代理
    public class ModuleServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _originalProvider;
        private readonly IModuleServiceResolver _moduleServiceResolver;

        public ModuleServiceProvider(IServiceProvider originalProvider, IModuleServiceResolver moduleServiceResolver)
        {
            _originalProvider = originalProvider;
            _moduleServiceResolver = moduleServiceResolver;
        }

        public object? GetService(Type serviceType)
        {
            var service = _moduleServiceResolver.ResolveService(serviceType);
            if (service != null)
            {
                return service;
            }
            return _originalProvider.GetService(serviceType);
        }
    }
    public interface IModuleFactory
    {
        IModule CreateModule(Type moduleType);
        IModule CreateModule<T>() where T : class, IModule;
    }
}
