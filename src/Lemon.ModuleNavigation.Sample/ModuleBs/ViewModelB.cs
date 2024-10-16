using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ModuleBs
{
    public class ViewModelB : ViewModelBase, IViewModel
    {
        private readonly NavigationService _navigationService;
        public ViewModelB(NavigationService navigationService)
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
        public override string Greeting => $"{base.Greeting}:{Environment.NewLine}{DateTime.Now}";
    }
}
