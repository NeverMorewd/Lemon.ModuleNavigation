﻿using Lemon.ModuleNavigation.Abstractions;

namespace Lemon.ModuleNavigation;

public class ServiceProviderDecorator : IServiceProviderDecorator
{
    private readonly IServiceProvider _serviceProvider;
    public ServiceProviderDecorator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? GetService(Type serviceType)
    {
        return GetServiceInternal(serviceType);
    }

    private object? GetServiceInternal(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
}
