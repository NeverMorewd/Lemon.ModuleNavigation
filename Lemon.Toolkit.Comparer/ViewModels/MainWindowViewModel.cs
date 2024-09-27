using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using DynamicData;
using Lemon.Toolkit.Comparer.Models;
using Lemon.Toolkit.Comparer.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Cryptography;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace Lemon.Toolkit.Comparer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase,IDisposable
    {
        private const int MaxOutputCount = 200;
        private readonly CompositeDisposable _disposables;

        private readonly TopLevelService _topLevelService;
        private readonly ConsoleService _consoleService;
        private WindowNotificationManager? _notificationManager;

        private readonly SourceCache<ConsoleTextModel, Guid> _outputsCache = new(x => x.Id);
        private readonly ReadOnlyObservableCollection<ConsoleTextModel> _outputs;
        public MainWindowViewModel(TopLevelService topLevelService, 
            ConsoleService consoleService)
        {
            _topLevelService = topLevelService;
            _consoleService = consoleService;

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

            CopyCommand = ReactiveCommand.CreateFromTask<object>(async (obj) =>
            {
                string? content = null;
                if (obj is string text)
                {
                    content = text;
                }
                else if (obj is IEnumerable objects)
                {
                    var texts = objects.Cast<string>();
                    content = string.Join('-', texts);
                }
                if (string.IsNullOrEmpty(content))
                {
                    NotificationManager!.Show(new Notification("复制失败", "内容为空", NotificationType.Error));
                }
                else
                {
                    await _topLevelService.Ensure().Clipboard!.SetTextAsync(content);
                    NotificationManager!.Show(new Notification("复制成功", content, NotificationType.Success));
                }
            });

            BrowseFileCommand = ReactiveCommand.CreateFromTask<Unit, string?>(async _ =>
            {
                FilePickerOpenOptions options = new()
                {
                    AllowMultiple = false
                };
                var files = await _topLevelService.Ensure().StorageProvider.OpenFilePickerAsync(options);
                if (files != null && files.Any())
                {
                    return files[0].TryGetLocalPath();
                }
                return null;
            });

            ClearOutputCommand = ReactiveCommand.Create(() => { _outputsCache.Clear(); });
            CopyOutputCommand = ReactiveCommand.CreateFromTask(async () => 
            {
                if(_outputs.Count < 1)
                {  return; }
                var texts = _outputs.Select(o => o.Text);
                var outputString = string.Join(Environment.NewLine, texts);
                await _topLevelService.Ensure().Clipboard!.SetTextAsync(outputString).ContinueWith(t =>
                {
                    NotificationManager!.Show(new Notification("成功", "已复制到剪切板", NotificationType.Success));
                });
            });
            BrowseFileCommand
                .Do(f => FilePath = f)
                //.ObserveOn(RxApp.TaskpoolScheduler)
                .WhereNotNull()
                .Where(File.Exists)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(f => { IsProcessing = true; })
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Select(f => (ComputeHash(f, MD5.Create()), ComputeHash(f, SHA256.Create()), ComputeFileSize(f)))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(hashes =>
                {
                    MD5Text = hashes.Item1;
                    SHA256Text = hashes.Item2;
                    FileSize = $"{hashes.Item3} MB";
                    IsProcessing = false;
                });

            _disposables = new(cacheCleanup, cacheCountCleanup, consoleOutputCleanup);

        }

        [Reactive]
        public string? FilePath
        {
            get;
            set;
        }
        [Reactive]
        public string? FileSize
        {
            get;
            set;
        }

        [Reactive]
        public string? MD5Text
        {
            get;
            set;
        }
        [Reactive]
        public string? SHA256Text
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
        public ReactiveCommand<Unit, string?> BrowseFileCommand
        {
            get;
        }
        public ReactiveCommand<object, Unit> CopyCommand
        {
            get;
        }
        public ReactiveCommand<Unit, Unit> ClearOutputCommand
        {
            get;
        }
        public ReactiveCommand<Unit, Unit> CopyOutputCommand
        {
            get;
        }
        public WindowNotificationManager? NotificationManager
        {
            get
            {
                if (_notificationManager == null)
                {
                    if (_topLevelService.Get() != null)
                    {
                        _notificationManager = new WindowNotificationManager(_topLevelService.Get()!)
                        {
                            MaxItems = 3,
                            Position = NotificationPosition.BottomRight
                        };
                    }
                }
                return _notificationManager;
            }
        }

        static string ComputeHash(string filePath, HashAlgorithm hashAlgorithm)
        {
            using (hashAlgorithm)
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hashBytes = hashAlgorithm.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
        static double ComputeFileSize(string filePath)
        {
            FileInfo fileInfo = new(filePath);
            long fileSizeInBytes = fileInfo.Length;
            double fileSizeInMB = fileSizeInBytes / (1024.0 * 1024.0);
            Console.WriteLine($"文件大小: {fileSizeInMB} MB");
            return Math.Round(fileSizeInMB, 2);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
