using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ModuleCs
{
    public class ViewModelC : SampleViewModelBase, IModuleNavigationAware, IServiceAware
    {
        private readonly IModuleNavigationService<IModule> _navigationService;
        private readonly IServiceProvider _moduleServiceProvider;
        public ViewModelC(IModuleNavigationService<IModule> navigationService, 
            IModuleServiceProvider moduleServiceProvider) 
        {
            _navigationService = navigationService;
            _moduleServiceProvider = moduleServiceProvider;
            NavigateCommand = ReactiveCommand.Create<string>(target => 
            {
                _navigationService.RequestModuleNavigate(target, null);
            });
        }

        public ReactiveCommand<string, Unit> NavigateCommand
        {
            get;
        }

        public IServiceProvider ServiceProvider => _moduleServiceProvider;
    }
}
