using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ModuleCs
{
    public class ViewModelC : ViewModelBase, IViewModel
    {
        private readonly NavigationService _navigationService;
        public ViewModelC(NavigationService navigationService) 
        {
            _navigationService = navigationService;
            NavigateCommand = ReactiveCommand.Create<string>(target => 
            {
                _navigationService.NavigateTo(target);
            });
        }

        public override string Greeting => $"{base.Greeting}:{Environment.NewLine}{DateTime.Now}";

        public ReactiveCommand<string, Unit> NavigateCommand
        {
            get;
        }
    }
}
