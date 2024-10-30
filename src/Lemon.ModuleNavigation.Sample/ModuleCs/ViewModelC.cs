using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.ModuleNavigation.Sample.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ModuleCs
{
    public class ViewModelC : SampleViewModelBase, IViewModel, INavigationProvider
    {
        private readonly IModuleNavigationService<IModule> _navigationService;
        private readonly IServiceProvider _moduleServiceProvider;
        public ViewModelC(IModuleNavigationService<IModule> navigationService, 
            IModuleServiceProvider moduleServiceProvider) 
        {
            _navigationService = navigationService;
            _moduleServiceProvider = moduleServiceProvider;
            NavigationHandler = _moduleServiceProvider.GetRequiredService<INavigationHandler>();
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
        public INavigationHandler NavigationHandler
        {
            get;
        }
    }
}
