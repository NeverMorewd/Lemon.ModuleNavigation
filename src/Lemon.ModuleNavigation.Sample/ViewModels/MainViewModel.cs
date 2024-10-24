using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.ModuleNavigation.Sample.Views;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ViewModels;

public class MainViewModel : SampleViewModelBase, INavigationContextProvider
{
    private readonly NavigationService _navigationService;
    public MainViewModel(AvaNavigationContext navigationContext,
        IEnumerable<IModule> modules,
        NavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigationContext = navigationContext;
        Modules = new ObservableCollection<IModule>(modules);
        ToViewCommand = ReactiveCommand.Create(() => 
        {
            _navigationService.NavigateToView<ViewAlpha>("NContentContainer");
        });
    }
    public ReactiveCommand<Unit, Unit> ToViewCommand
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
