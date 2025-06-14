﻿using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.SampleViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleCs.ViewModels;

public class SubViewModel01 : BaseNavigationViewModel, IModuleNavigationAware
{
    private readonly ILogger _logger;
    public SubViewModel01(IServiceProvider serviceProvider, IServiceProviderDecorator appServiceProvider)
    {
        _logger = appServiceProvider.GetRequiredService<ILogger<SubViewModel01>>();
        _logger.LogDebug("SubViewModel01");
    }
}
