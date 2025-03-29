﻿using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Lemon.ModuleNavigation.Dialogs;
using Lemon.ModuleNavigation.Extensions;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ViewModels;

public class MainViewModel : SampleViewModelBase, IServiceAware
{
    private readonly NavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDialogService _dialogService;
    private readonly IRegionManager _regionManager;
    private readonly ILogger _logger;
    public MainViewModel(IEnumerable<IModule> modules,
        IServiceProvider serviceProvider,
        NavigationService navigationService,
        IDialogService dialogService,
        IRegionManager regionManager,
        ILogger<MainViewModel> logger)
    {
        _logger = logger;
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
        _dialogService = dialogService;
        _regionManager = regionManager;
        // default views for different regions
        _navigationService.RequestViewNavigation("ContentRegion", "ViewAlpha", false);
        _navigationService.RequestViewNavigation("TransitioningContentRegion", "ViewAlpha", false);
        Modules = new ObservableCollection<IModule>(modules);
        ToViewCommand = ReactiveCommand.Create<string>(content => 
        {
            var viewName = content;
            var requestNew = false;
            if (content.EndsWith(".RequestNew"))
            {
                viewName = content.Replace(".RequestNew",string.Empty);
                requestNew = true;

            }
            _navigationService.RequestViewNavigation("ContentRegion", viewName, requestNew);
            _navigationService.RequestViewNavigation("TabRegion", viewName, requestNew);
            _navigationService.RequestViewNavigation("ItemsRegion", viewName, requestNew);
            _navigationService.RequestViewNavigation("TransitioningContentRegion", viewName, requestNew);
        });
        ShowCommand = ReactiveCommand.Create<string>(content =>
        {
            var param = new DialogParameters
            {
                { "parent", nameof(MainViewModel) }
            };
            _dialogService.Show(content, null, param, result =>
            {
                _logger.LogDebug($"Call back:{result}");
            });
            _logger.LogDebug($"Show over!");
        });

        ShowDialogCommand = ReactiveCommand.CreateFromTask<string>(async content =>
        {
            var param = new DialogParameters
            {
                { "parent", nameof(MainViewModel) }
            };
            await _dialogService.ShowDialog(content, 
                nameof(CustomDialogWindow), 
                param,
                result =>
                {
                    _logger.LogDebug($"Call back:{result}");
                });
            _logger.LogDebug($"ShowDialog over!");
        });

        ShowDialogSyncCommand = ReactiveCommand.Create<string>(content => 
        {
            var param = new DialogParameters
            {
                { "parent", nameof(MainViewModel) }
            };
            var result = _dialogService.WaitShowDialog(content, 
                nameof(CustomDialogWindow), 
                param,
                result =>
                {
                    _logger.LogDebug($"Call back:{result}");
                });
            _logger.LogDebug($"ShowDialog over:{result}");
        });

        _regionManager.NavigationSubscribe<NavigationContext>(n => 
        {
            _logger.LogDebug($"Request to : {n.RegionName}.{n.TargetViewName}");
        });
        _regionManager.NavigationSubscribe<IRegion>(r => 
        {
            _logger.LogDebug($"New region : {r.Name}");
        });
    }
    public ReactiveCommand<string, Unit> ToViewCommand
    {
        get;
        set;
    }
    public ReactiveCommand<string, Unit> ShowCommand
    {
        get;
        set;
    }
    public ReactiveCommand<string, Unit> ShowDialogCommand
    {
        get;
        set;
    }
    public ReactiveCommand<string, Unit> ShowDialogSyncCommand
    {
        get;
        set;
    }
    public ObservableCollection<IModule> Modules
    {
        get;
        set;
    }

    private IModule? _selectedModule;
    public IModule? SelectedModule
    {
        get => _selectedModule;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedModule, value);
            _navigationService.RequestModuleNavigate(_selectedModule!, null);
        }
    }
    public IServiceProvider ServiceProvider => _serviceProvider;
}
