using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Abstracts
{
    public class ServiceDescriptorsAttribute : Attribute
    {
        public ServiceDescriptorsAttribute() 
        {

        }
        public IEnumerable<ServiceDescription> ServiceDescriptions
        {
            get;
            set;
        }
        public IEnumerable<NavigationDescription> NavigationDescriptions
        {
            get;
            set;
        }
    }
    public class ServiceDescription
    {
        public ServiceDescription(Type type, ServiceLifetime lifetime, object? key = null)
        {
            Key = key;
            ServiceLifetime = lifetime;
            ServiceType = type;            
        }
        public object? Key
        {
            get;
            private set;
        }
        public Type ServiceType
        {
            get;
            private set;
        }
        public ServiceLifetime ServiceLifetime
        {
            get;
            private set;
        }
    }
    public class NavigationDescription
    {
        public NavigationDescription(Type viewType, Type viewModelType, ServiceLifetime lifetime, object? key = null)
        {
            Key = key;
            ServiceLifetime = lifetime;
            ViewType = viewType;
            ViewModelType = viewModelType;
        }
        public object? Key
        {
            get;
            private set;
        }
        public Type ViewType
        {
            get;
            private set;
        }
        public Type ViewModelType
        {
            get;
            private set;
        }
        public ServiceLifetime ServiceLifetime
        {
            get;
            private set;
        }
    }
}
