using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ModuleAs
{
    public class ViewModelA : SampleViewModelBase, IViewModel
    {
        private readonly NavigationService _navigationService;
        public ViewModelA(NavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = ReactiveCommand.Create<string>(target =>
            {
                _navigationService.NavigateTo(target);
            });
        }
        public ReactiveCommand<string, Unit> NavigateCommand
        {
            get;
        }
        public override string Greeting => $"{base.Greeting}:Load immediately{Environment.NewLine}{DateTime.Now}";
    }
}
