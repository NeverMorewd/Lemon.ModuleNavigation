using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation
{
    public class AppServiceProvider : IAppServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public AppServiceProvider(IServiceProvider serviceProvider)
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
