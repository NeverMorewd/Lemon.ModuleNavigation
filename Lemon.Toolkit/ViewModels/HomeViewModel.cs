using Lemon.Toolkit.Framework;
using Lemon.Toolkit.Framework.Abstracts;
using Lemon.Toolkit.Services;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;

namespace Lemon.Toolkit.ViewModels
{
    [RequiresUnreferencedCode("")]
    public class HomeViewModel:ViewModelBase
    {
        private readonly TopLevelService _topLevelService;
        private readonly NavigationService _navigationService;
        private readonly ILogger _logger;
        public HomeViewModel(TopLevelService topLevelService,
            IEnumerable<IModule> modules,
            NavigationService navigationService,
            ILogger<HomeViewModel> logger) 
        {
            _navigationService = navigationService;
            _topLevelService = topLevelService;
            _logger = logger;
            Modules = new ObservableCollection<IModule>(modules);
            this.ObservableForProperty(x => x.SelectedItem)
                .WhereNotNull()
                .Where(x=>x.Value is not null)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(c => 
                {
                    _logger.LogDebug($"navigate to {c.Value!.Name}");
                    _navigationService.NavigateTo(c.Value!);
                    SelectedItem = null;
                });
        }
        public ObservableCollection<IModule> Modules
        {
            get;
            set;
        }
        [Reactive]
        public IModule? SelectedItem
        {
            get;
            set;
        }
    }
}
