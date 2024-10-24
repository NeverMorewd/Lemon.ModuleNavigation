using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.ModuleNavigation.Sample.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ViewModels;

public class MainViewModel : SampleViewModelBase, INavigationContextProvider
{
    private readonly NavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;
    public MainViewModel(AvaNavigationContext navigationContext,
        IEnumerable<IModule> modules,
        IServiceProvider serviceProvider,
        NavigationService navigationService)
    {
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
        NavigationContext = navigationContext;
        Modules = new ObservableCollection<IModule>(modules);
        //Views = new ObservableCollection<UserControl>(views);
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
        });
    }
    public ReactiveCommand<string, Unit> ToViewCommand
    {
        get;
        set;
    }
    /// <summary>
    /// For binding
    /// </summary>
    public ObservableCollection<IModule> Modules
    {
        get;
        set;
    }
    public IEnumerable<string> ViewNames
    {
        get;
        set;
    }
    private UserControl? _selectedView;
    public UserControl? SelectedView
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
    public INavigationContext NavigationContext
    {
        get;
    }
}
