using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Sample.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ModuleBs;

public class ViewModelB : BaseNavigationViewModel, IModuleNavigationAware
{
    private readonly NavigationService _navigationService;
    public ViewModelB(NavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigateCommand = ReactiveCommand.Create<string>(target =>
        {
            _navigationService.RequestModuleNavigate(target, null);
        });
    }
    public ReactiveCommand<string, Unit> NavigateCommand
    {
        get;
    }
}
