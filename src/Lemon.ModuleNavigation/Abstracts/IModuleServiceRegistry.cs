using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Abstracts
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ServiceDescriptorsAttribute : Attribute
    {
        public ServiceDescriptorsAttribute(Type[] serviceTypes, ServiceLifetime[] serviceLifetimes, object[]? keys = null)
        {
            ServiceTypes = serviceTypes;
            ServiceLifetimes = serviceLifetimes;
            Keys = keys ?? Array.Empty<object>();
        }

        public Type[] ServiceTypes { get; }
        public ServiceLifetime[] ServiceLifetimes { get; }
        public object[] Keys { get; }
    }

    //public class ServiceDescriptorsAttribute : Attribute
    //{
    //    public ServiceDescriptorsAttribute(params (Type, ServiceLifetime, object?)[] serviceDescriptions) 
    //    {
    //        ServiceDescriptions = serviceDescriptions;
    //    }
    //    public IEnumerable<(Type, ServiceLifetime, object?)> ServiceDescriptions
    //    {
    //        get;
    //        private set;
    //    }
    //}
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
