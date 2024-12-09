using Lemon.ModuleNavigation.Abstracts;
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
        // default view
        _navigationService.RequestViewNavigation("ContentRegion", "ViewAlpha", false);
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
        ToDialogCommand = ReactiveCommand.Create<string>(async content =>
        {
            var viewName = content;
            var showDialog = false;
            if (content.EndsWith(".ShowDialog"))
            {
                viewName = content.Replace(".ShowDialog", string.Empty);
                showDialog = true;

            }
            var param = new DialogParameters
            {
                { "start", nameof(MainViewModel) }
            };
            if (showDialog)
            {
                await _dialogService.ShowDialog(viewName, nameof(CustomDialogWindow), param, p =>
                {
                    _logger.LogDebug($"Call back:{p}");
                });
                _logger.LogDebug($"ShowDialog over!");
            }
            else
            {
                _dialogService.Show(viewName, param, p =>
                {
                    _logger.LogDebug($"close call back:{p}");
                });
            }
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
    public ReactiveCommand<string, Unit> ToDialogCommand
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
