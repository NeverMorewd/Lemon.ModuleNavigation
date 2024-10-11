using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using Lemon.Toolkit.Models;
using Lemon.Toolkit.Services;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Cryptography;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace Lemon.Toolkit.ViewModels
{
    [RequiresUnreferencedCode("")]
    public class FileInspectorViewModel: ViewModelBase
    {
        private readonly CompositeDisposable? _disposables;
        private readonly TopLevelService _topLevelService;
        private readonly IObserver<ShellParamModel> _shellService;
        private readonly ILogger _logger;
        public FileInspectorViewModel(TopLevelService topLevelService, 
            IObserver<ShellParamModel> shellService,
            ILogger<FileInspectorViewModel> logger) 
        {
            _logger = logger;
            _topLevelService = topLevelService;
            _shellService = shellService;
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
                if (string.IsNullOrEmpty(content) || content == "-")
                {
                    _logger.LogError("Can not copy empty string");
                    _topLevelService.Ensure().NotificationManager!.Show(new Notification("Error", "Can not copy empty string!", NotificationType.Error));
                }
                else
                {
                    await _topLevelService.EnsureTopLevel().Clipboard!.SetTextAsync(content);
                    _topLevelService.NotificationManager!.Show(new Notification("Success", "Copied", NotificationType.Success));
                }
            });
            BrowseFileCommand = ReactiveCommand.CreateFromTask<Unit, string?>(async _ =>
            {
                FilePickerOpenOptions options = new()
                {
                    AllowMultiple = false
                };
                var files = await _topLevelService.EnsureTopLevel().StorageProvider.OpenFilePickerAsync(options);
                if (files != null && files.Any())
                {
                    return files[0].TryGetLocalPath();
                }
                return null;
            });
            BrowseFileCommand
                .Do(f => FilePath = f)
                //.ObserveOn(RxApp.TaskpoolScheduler)
                .WhereNotNull()
                .Where(File.Exists)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Do(f => { _shellService.OnNext(new ShellParamModel { IsProcessing = true }); })
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Select(f => (ComputeHash(f, MD5.Create()), ComputeHash(f, SHA256.Create()), ComputeFileSize(f)))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(hashes =>
                {
                    MD5Text = hashes.Item1;
                    SHA256Text = hashes.Item2;
                    FileSize = $"{hashes.Item3} MB";
                    _shellService.OnNext(new ShellParamModel { IsProcessing = false });
                });
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

        public ReactiveCommand<Unit, string?> BrowseFileCommand
        {
            get;
        }
        public ReactiveCommand<object, Unit> CopyCommand
        {
            get;
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
    }
}
