using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Lemon.Toolkit.Services
{
    public class TopLevelService
    {
        private TopLevel? _topLevel;
        private WindowNotificationManager? _notificationManager;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        public TopLevelService()
        {

        }

        public TopLevel? Get()
        {
            return GetTopLevelCore();
        }

        public async Task<TopLevel> EnsureAync(int maxAttempts = 10, int delayMilliseconds = 10)
        {
            if (_topLevel == null)
            {
                try
                {
                    await _semaphore.WaitAsync();
                    var ob = Observable.Generate(0,
                                                i => i < maxAttempts,
                                                i => i + 1,
                                                i => GetTopLevelCore(),
                                                i => TimeSpan.FromMilliseconds(delayMilliseconds))
                                .Where(topLevel => topLevel != null)
                                .Take(1);
                    _topLevel = await ob.ToTask();
                    if (_topLevel == null)
                    {
                        throw new InvalidOperationException();
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            return _topLevel;
        }
        public TopLevel Ensure(int maxAttempts = 10, int delayMilliseconds = 10)
        {
            if (_topLevel == null)
            {
                lock (this)
                {
                    var ob = Observable.Generate(0,
                                                i => i < maxAttempts,
                                                i => i + 1,
                                                i => GetTopLevelCore(),
                                                i => TimeSpan.FromMilliseconds(delayMilliseconds))
                            .Where(topLevel => topLevel != null)
                            .Take(1);
                    _topLevel = ob.Wait();
                    if (_topLevel == null)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            return _topLevel;
        }
        public WindowNotificationManager? NotificationManager
        {
            get
            {
                if (_notificationManager == null)
                {
                    if (_topLevel != null)
                    {
                        _notificationManager = new WindowNotificationManager(_topLevel!)
                        {
                            MaxItems = 3,
                            Position = NotificationPosition.BottomRight
                        };
                    }
                }
                return _notificationManager;
            }
        }

        private TopLevel? GetTopLevelCore()
        {
            if (_topLevel != null)
            {
                return _topLevel;
            }
            if (Avalonia.Application.Current is not null)
            {
                if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    if (desktop.MainWindow is not null)
                    {
                        _topLevel = TopLevel.GetTopLevel(desktop.MainWindow);
                        return _topLevel;
                    }
                }
            }
            return null;
        }
    }
}
