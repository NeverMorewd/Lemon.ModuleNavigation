using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Lemon.ModuleNavigation.AdvancedDI
{
    /// <summary>
    /// Service provider that allows for dynamic adding of new services
    /// </summary>
    public class AdvancedServiceProvider : IAdvancedServiceProvider, IDisposable
    {
        private readonly List<ServiceProvider> _serviceProviders;
        private readonly NotifyChangedServiceCollection _services;
        private readonly object _servicesLock = new object();
        private List<ServiceDescriptor> _newDescriptors;
        private readonly Dictionary<Type, object> _resolvedObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedServiceProvider"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public AdvancedServiceProvider(IServiceCollection services)
        {
            // registers itself in the list of services
            services.AddSingleton<IAdvancedServiceProvider>(this);

            _serviceProviders = [];
            _newDescriptors = [];
            _resolvedObjects = [];
            _services = new NotifyChangedServiceCollection(services);
            _services.ServiceAdded += ServiceAdded;
            _serviceProviders.Add(services.BuildServiceProvider(true));
        }

        private void ServiceAdded(object? sender, ServiceDescriptor item)
        {
            lock (_servicesLock)
            {
                _newDescriptors.Add(item);
            }
        }

        /// <summary>
        /// Add services to this collection
        /// </summary>
        public IServiceCollection ServiceCollection { get => _services; }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType. -or- null if there is no service object of type serviceType.</returns>
        public object? GetService(Type serviceType)
        {
            lock (_servicesLock)
            {
                // go through the service provider chain and resolve the service
                var service = GetServiceInternal(serviceType);
                // if service was not found and we have new registrations
                if (service == null && _newDescriptors.Count > 0)
                {
                    // create a new service collection in order to build the next provider in the chain
                    var newCollection = new ServiceCollection();
                    foreach (var descriptor in _services)
                    {
                        foreach (var descriptorToAdd in GetDerivedServiceDescriptors(descriptor))
                        {
                            ((IList<ServiceDescriptor>)newCollection).Add(descriptorToAdd);
                        }
                    }
                    var newServiceProvider = newCollection.BuildServiceProvider(true);
                    _serviceProviders.Add(newServiceProvider);
                    _newDescriptors = [];
                    service = newServiceProvider.GetService(serviceType);
                }
                if (service != null)
                {
                    _resolvedObjects[serviceType] = service;
                }
                return service;
            }
        }

        private IEnumerable<ServiceDescriptor> GetDerivedServiceDescriptors(ServiceDescriptor descriptor)
        {
            if (_newDescriptors.Contains(descriptor))
            {
                // if it's a new registration, just add it
                yield return descriptor;
                yield break;
            }

            if (!descriptor.ServiceType.IsGenericTypeDefinition)
            {
                // for a non open type generic singleton descriptor, register a factory that goes through the service provider
                yield return ServiceDescriptor.Describe(
                                        descriptor.ServiceType,
                                        _ => GetServiceInternal(descriptor.ServiceType)!,
                                        descriptor.Lifetime
                                    );
                yield break;
            }
            // if the registered service type for a singleton is an open generic type
            // we register as factories all the already resolved specific types that fit this definition
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                foreach (var servType in _resolvedObjects.Keys.Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == descriptor.ServiceType))
                {

                    yield return ServiceDescriptor.Describe(
                            servType,
                            _ => _resolvedObjects[servType],
                            ServiceLifetime.Singleton
                        );
                }
            }
            // then we add the open type registration for any new types
            yield return descriptor;
        }

        private object? GetServiceInternal(Type serviceType)
        {
            foreach (var serviceProvider in _serviceProviders)
            {
                var service = serviceProvider.GetService(serviceType);
                if (service != null)
                {
                    return service;
                }
            }
            return null;
        }

        /// <summary>
        /// Dispose the provider and all resolved services
        /// </summary>
        public void Dispose()
        {
            lock (_servicesLock)
            {
                _services.ServiceAdded -= ServiceAdded;
                foreach (var serviceProvider in _serviceProviders)
                {
                    try
                    {
                        serviceProvider.Dispose();
                    }
                    catch
                    {
                        // singleton classes might be disposed twice and throw some exception
                    }
                }
                _newDescriptors.Clear();
                _resolvedObjects.Clear();
                _serviceProviders.Clear();
            }
        }

        /// <summary>
        /// An IServiceCollection implementation that exposes a ServiceAdded event for added service descriptors
        /// The collection doesn't support removal or inserting of services
        /// </summary>
        private class NotifyChangedServiceCollection : IServiceCollection
        {
            private readonly IServiceCollection _services;

            /// <summary>
            /// Fired when a descriptor is added to the collection
            /// </summary>
            public event EventHandler<ServiceDescriptor>? ServiceAdded;

            /// <summary>
            /// Initializes a new instance of the <see cref="NotifyChangedServiceCollection"/> class.
            /// </summary>
            /// <param name="services">The services.</param>
            public NotifyChangedServiceCollection(IServiceCollection services)
            {
                _services = services;
            }

            /// <summary>
            /// Get the value at index
            /// Setting is not supported
            /// </summary>
            public ServiceDescriptor this[int index]
            {
                get => _services[index];
                set => throw new NotSupportedException("Inserting services in collection is not supported");
            }

            /// <summary>
            /// Count of services in the collection
            /// </summary>
            public int Count { get => _services.Count; }

            /// <summary>
            /// Obviously not
            /// </summary>
            public bool IsReadOnly { get => false; }

            /// <summary>
            /// Adding a service descriptor will fire the ServiceAdded event
            /// </summary>
            /// <param name="item"></param>
            public void Add(ServiceDescriptor item)
            {
                _services.Add(item);
                ServiceAdded?.Invoke(this, item);
            }

            /// <summary>
            /// Clear the collection is not supported
            /// </summary>
            public void Clear() => throw new NotSupportedException("Removing services from collection is not supported");

            /// <summary>
            /// True is the item exists in the collection
            /// </summary>
            public bool Contains(ServiceDescriptor item) => _services.Contains(item);

            /// <summary>
            /// Copy items to array of service descriptors
            /// </summary>
            public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => _services.CopyTo(array, arrayIndex);

            /// <summary>
            /// Enumerator for service descriptors
            /// </summary>
            public IEnumerator<ServiceDescriptor> GetEnumerator() => _services.GetEnumerator();

            /// <summary>
            /// Index of item in the list
            /// </summary>
            public int IndexOf(ServiceDescriptor item) => _services.IndexOf(item);

            /// <summary>
            /// Inserting is not supported
            /// </summary>
            public void Insert(int index, ServiceDescriptor item) => throw new NotSupportedException("Inserting services in collection is not supported");

            /// <summary>
            /// Removing items is not supported
            /// </summary>
            public bool Remove(ServiceDescriptor item) => throw new NotSupportedException("Removing services from collection is not supported");

            /// <summary>
            /// Removing items is not supported
            /// </summary>
            public void RemoveAt(int index) => throw new NotSupportedException("Removing services from collection is not supported");

            /// <summary>
            /// Enumerator for objects
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_services).GetEnumerator();
        }
    }
}
