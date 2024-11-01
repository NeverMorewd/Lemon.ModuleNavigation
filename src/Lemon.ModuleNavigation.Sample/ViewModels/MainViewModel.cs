using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Lemon.ModuleNavigation.Dialogs;
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
    private readonly ILogger _logger;
    public MainViewModel(IEnumerable<IModule> modules,
        IServiceProvider serviceProvider,
        NavigationService navigationService,
        IDialogService dialogService,
        ILogger<MainViewModel> logger)
    {
        _logger = logger;
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
        _dialogService = dialogService;
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
            _navigationService.NavigateToView("NContentContainer", viewName, requestNew);
            _navigationService.NavigateToView("NTabContainer", viewName, requestNew);
            _navigationService.NavigateToView("NItemsContainer", viewName, requestNew);
            _navigationService.NavigateToView("NTransitioningContentControl", viewName, requestNew);
        });
        ToDialogCommand = ReactiveCommand.Create<string>(content =>
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
                _dialogService.ShowDialog(viewName, param, p =>
                {
                    _logger.LogDebug($"close call back:{p}");
                });
            }
            else
            {
                _dialogService.Show(viewName, param, p =>
                {
                    _logger.LogDebug($"close call back:{p}");
                });
            }
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
    private IView? _selectedView;
    public IView? SelectedView
    {
        get => _selectedView;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedView, value);
            if (_selectedView != null)
            {
                var viewName = _selectedView.GetType().Name;
                _navigationService.NavigateToView("NContentContainer", viewName);
                _navigationService.NavigateToView("NTabContainer", viewName);
                _navigationService.NavigateToView("NItemsContainer", viewName);
            }
        }
    }

    private IModule? _selectedModule;
    public IModule? SelectedModule
    {
        get => _selectedModule;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedModule, value);
            _navigationService.NavigateTo(_selectedModule!);
        }
    }
    public IServiceProvider ServiceProvider => _serviceProvider;
}
