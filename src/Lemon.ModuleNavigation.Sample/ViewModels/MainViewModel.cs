using Lemon.ModuleNavigation.Abstracts;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Sample.ViewModels;

public class MainViewModel : SampleViewModelBase, INavigationContextProvider
{
    public readonly NavigationService _navigationService;
    public MainViewModel(NavigationContext navigationContext,
        IEnumerable<IModule> modules,
        NavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigationContext = navigationContext;
        Modules = new ObservableCollection<IModule>(modules);
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
    public NavigationContext NavigationContext
    {
        get;
    }
}
