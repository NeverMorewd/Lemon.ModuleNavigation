using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation
{

    public class ModuleServiceProvider : IModuleServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public ModuleServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object? GetService(Type serviceType)
        {
            return GetServiceInternal(serviceType);
        }

        private object? GetServiceInternal(Type serviceType)
        {
            var service = _serviceProvider.GetService(serviceType);
            return service;
        }
    }
}
