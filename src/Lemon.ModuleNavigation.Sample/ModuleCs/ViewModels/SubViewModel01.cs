﻿using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleCs.ViewModels
{
    public class SubViewModel01 : SampleViewModelBase, IViewModel
    {
        private readonly ILogger _logger;
        public SubViewModel01(IServiceProvider serviceProvider, IAppServiceProvider appServiceProvider)
        {
            _logger = appServiceProvider.GetRequiredService<ILogger<SubViewModel01>>();
            _logger.LogDebug("SubViewModel01");
        }
    }
}
