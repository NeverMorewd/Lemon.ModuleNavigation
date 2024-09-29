using DynamicData.Binding;
using Lemon.Toolkit.Framework;
using Lemon.Toolkit.Services;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System;

namespace Lemon.Toolkit.ViewModels
{
    [RequiresUnreferencedCode("")]
    public class HomeViewModel:ViewModelBase
    {
        private readonly ConsoleService _consoleService;
        private readonly TopLevelService _topLevelService;
        private readonly IObserver<ITabModule> _navigationService;
        public HomeViewModel(ConsoleService consoleService, 
            TopLevelService topLevelService,
            IEnumerable<ITabModule> modules,
            IObserver<ITabModule> navigationService) 
        {
            _navigationService = navigationService;
            _consoleService = consoleService;
            _topLevelService = topLevelService;
            Modules = new ObservableCollection<ITabModule>(modules);
            this.ObservableForProperty(x => x.SelectedItem)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(c => 
                {
                    _navigationService.OnNext(c.Value);
                });
        }
        public ObservableCollection<ITabModule> Modules
        {
            get;
            set;
        }
        [Reactive]
        public ITabModule SelectedItem
        {
            get;
            set;
        }
    }
}
