using Avalonia.Controls.Notifications;
using Avalonia.Media;
using DynamicData;
using Lemon.Extensions.SlimModule.Abstracts;
using Lemon.Toolkit.Models;
using Lemon.Toolkit.Services;
using Lemon.Toolkit.ViewModels;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace Lemon.Toolkit.Shells
{
    public class MainWindowViewModel : ViewModelBase,INavigationHandler<IModule>, IDisposable
    {
        private const int MaxOutputCount = 200;
        private readonly CompositeDisposable _disposables;

        private readonly TopLevelService _topLevelService;
        private readonly ConsoleService _consoleService;
        private readonly IObservable<ShellParamModel> _shellService;
        private readonly ILogger _logger;
        private readonly INavigationService<IModule> _navigationService;

        private readonly SourceCache<ConsoleTextModel, Guid> _outputsCache = new(x => x.Id);
        private readonly ReadOnlyObservableCollection<ConsoleTextModel> _outputs;
        public MainWindowViewModel(TopLevelService topLevelService,
            ConsoleService consoleService,
            IObservable<ShellParamModel> shellService,
            IEnumerable<IModule> modules,
            INavigationService<IModule> navigationService,
            ILogger<MainWindowViewModel> logger)
        {
            _logger = logger;
            _topLevelService = topLevelService;
            _consoleService = consoleService;
            _navigationService = navigationService;
            _shellService = shellService;

            using (_logger.BeginScope("Modules"))
            {
                Modules = new ObservableCollection<IModule>(modules
                    .Where(m =>
                    {
                        _logger.LogInformation($"Found module:{m.Key}");
                        return !m.LoadOnDemand;
                    })
                    .Select(m =>
                    {
                        _logger.LogInformation($"Initialize module:{m.Key}");
                        m.Initialize();
                        return m;
                    }));
            }

            #region Outputs Cache
            var cacheCleanup = _outputsCache.Connect()
                .LimitSizeTo(MaxOutputCount)
                .Bind(out _outputs)
                .DisposeMany()
                .Subscribe();

            var cacheCountCleanup = _outputsCache.Connect()
                .CountChanged()
                .Subscribe(cache =>
                {
                    if (ConsoleIsExpanded) return;
                    if (OutputCount == 100) return;
                    OutputCount += cache.Count;
                });
            var consoleOutputCleanup = consoleService
                .OutputObservable
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(outPut =>
                {
                    _outputsCache.AddOrUpdate(new ConsoleTextModel($"{outPut}"));
                });
            var consoleErrorCleanup = consoleService
                .ErrorObservable
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(outPut =>
                {
                   _outputsCache.AddOrUpdate(new ConsoleTextModel($"{outPut}", brush:new SolidColorBrush(Colors.Red)));
                });

            #endregion
            _shellService
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(param => 
                {
                    IsProcessing = param.IsProcessing;
                });
            ClearOutputCommand = ReactiveCommand.Create(() => _outputsCache.Clear());
            CopyOutputCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (_outputs.Count < 1)
                { 
                    return; 
                }
                var texts = _outputs.Select(o => o.Text);
                var outputString = string.Join(Environment.NewLine, texts);
                await _topLevelService.EnsureTopLevel().Clipboard!.SetTextAsync(outputString);
                _topLevelService.Ensure().NotificationManager!.Show(new Notification("Success", "Copied!", NotificationType.Success));
            });

            var valueChangedCleanup = this.WhenAnyValue(x => x.ConsoleIsExpanded)
                .Subscribe(c=>
                {
                    if (c)
                    {
                        OutputCount = 0;
                    }
                });
            var navigationCleanup = _navigationService.OnNavigation(this);

            _disposables = new(cacheCleanup, 
                cacheCountCleanup, 
                consoleOutputCleanup, 
                valueChangedCleanup, 
                consoleErrorCleanup,
                navigationCleanup);

        }
        public ObservableCollection<IModule> Modules
        {
            get;
            set;
        }
        [Reactive]
        public bool IsProcessing
        {
            get;
            set;
        }
        [Reactive]
        public int OutputCount
        {
            get;
            set;
        }
        [Reactive]
        public bool ConsoleIsExpanded
        {
            get;
            set;
        }
        [Reactive]
        public IModule? CurrentTab
        {
            get;
            set;
        }

        public ReadOnlyObservableCollection<ConsoleTextModel> Outputs
        {
            get => _outputs;
        }
        public ReactiveCommand<Unit, Unit> ClearOutputCommand
        {
            get;
        }
        public ReactiveCommand<Unit, Unit> CopyOutputCommand
        {
            get;
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }

        public void NavigateTo(IModule target)
        {
            if (!Modules.Contains(target))
            {
                Modules.Add(target);
            }
            target.Initialize();
            target.IsActivated = true;
            CurrentTab = target;
        }
    }
}
