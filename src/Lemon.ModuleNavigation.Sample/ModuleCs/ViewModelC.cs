using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ModuleCs
{
    public class ViewModelC : SampleViewModelBase, IViewModel, INavigationContextProvider
    {
        private readonly INavigationService<IModule> _navigationService;
        private readonly IServiceProvider _moduleServiceProvider;
        public ViewModelC(INavigationService<IModule> navigationService, 
            IModuleServiceProvider moduleServiceProvider) 
        {
            _navigationService = navigationService;
            _moduleServiceProvider = moduleServiceProvider;
            NavigationContext = _moduleServiceProvider.GetRequiredService<NavigationContext>();
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

        /// <summary>
        /// Sub NavigationContext
        /// </summary>
        public NavigationContext NavigationContext
        {
            get;
        }
    }
}
