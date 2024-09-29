using Avalonia.Controls.Notifications;
using DynamicData;
using Lemon.Toolkit.Framework;
using Lemon.Toolkit.Models;
using Lemon.Toolkit.Services;
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

namespace Lemon.Toolkit.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private const int MaxOutputCount = 200;
        private readonly CompositeDisposable _disposables;

        private readonly TopLevelService _topLevelService;
        private readonly ConsoleService _consoleService;
        private readonly IObservable<ITabModule> _navigationService;


        private readonly SourceCache<ConsoleTextModel, Guid> _outputsCache = new(x => x.Id);
        private readonly ReadOnlyObservableCollection<ConsoleTextModel> _outputs;
        public MainWindowViewModel(TopLevelService topLevelService,
            ConsoleService consoleService,
            IEnumerable<ITabModule> modules,
            IObservable<ITabModule> navigationService)
        {
            _topLevelService = topLevelService;
            _consoleService = consoleService;
            _navigationService = navigationService;
            Modules = new ObservableCollection<ITabModule>(modules.Where(m => m.LoadDefault).Select(m => { m.Initialize(); return m; }));

            #region Outputs Cache

            _outputsCache.AddOrUpdate(new ConsoleTextModel(Guid.NewGuid(), "### 启动 ###"));

            var cacheCleanup = _outputsCache.Connect()
                .LimitSizeTo(MaxOutputCount)
                .Bind(out _outputs)
                .DisposeMany()
                .Subscribe();

            var cacheCountCleanup = _outputsCache.Connect()
                .CountChanged()
                .Subscribe(set =>
                {
                    if (ConsoleIsExpanded) return;
                    if (OutputCount == 100)
                    {
                        return;
                    }
                    OutputCount += set.Count;
                });
            var consoleOutputCleanup = consoleService.OutputObservable
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(outPut =>
                {
                    _outputsCache.AddOrUpdate(new ConsoleTextModel(Guid.NewGuid(), $"{outPut}"));

                });

            #endregion

            ClearOutputCommand = ReactiveCommand.Create(() => { _outputsCache.Clear(); });
            CopyOutputCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (_outputs.Count < 1)
                { return; }
                var texts = _outputs.Select(o => o.Text);
                var outputString = string.Join(Environment.NewLine, texts);
                await _topLevelService.Ensure().Clipboard!.SetTextAsync(outputString).ContinueWith(t =>
                {
                    _topLevelService.NotificationManager!.Show(new Notification("成功", "已复制到剪切板", NotificationType.Success));
                });
            });
            _navigationService.ObserveOn(RxApp.MainThreadScheduler).Subscribe(m => 
            {
                m.Initialize();
                Modules.Add(m);
            });
            _disposables = new(cacheCleanup, cacheCountCleanup, consoleOutputCleanup);

        }
        public ObservableCollection<ITabModule> Modules
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
        
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
