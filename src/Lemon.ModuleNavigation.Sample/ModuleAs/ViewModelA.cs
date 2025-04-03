using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.SampleViewModel;
using ReactiveUI;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ModuleAs;

public class ViewModelA : BaseNavigationViewModel, IModuleNavigationAware
{
    private readonly NavigationService _navigationService;
    public ViewModelA(NavigationService navigationService)
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
