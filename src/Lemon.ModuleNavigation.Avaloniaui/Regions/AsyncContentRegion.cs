using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Threading;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions
{
    public class AsyncContentRegion : IAsyncRegion, INotifyPropertyChanged
    {
        private readonly ContentControl _contentControl;
        private readonly AsyncContentRegionTemplateSelector _templateSelector;
        private readonly ConcurrentDictionary<int, Task> _activationTasks = new();

        public AsyncContentRegion(string name, ContentControl contentControl)
        {
            _contentControl = contentControl;
            _templateSelector = new AsyncContentRegionTemplateSelector(this);
            _contentControl.DataTemplates.Add(_templateSelector);
            SetBindingContent();
        }

        private object? _content;
        public object? Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isNavigating;
        public bool IsNavigating
        {
            get => _isNavigating;
            private set
            {
                if (_isNavigating != value)
                {
                    _isNavigating = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name => throw new NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<NavigationEventArgs>? NavigationCompleted;
        public event EventHandler<NavigationErrorEventArgs>? NavigationFailed;

        //public void Activate(NavigationContext target)
        //{
        //    if (_activationTasks.ContainsKey(target.ViewKey))
        //        return;

        //    var activationTask = ActivateAsync(target);
        //    _activationTasks.TryAdd(target.ViewKey, activationTask);
        //    _ = activationTask.ContinueWith(t =>
        //    {
        //        _activationTasks.TryRemove(target.ViewKey, out _);
        //    }, TaskScheduler.Default);
        //}
        public async Task ActivateAsync(NavigationContext target)
        {
            try
            {
                await AvaloniauiExtensions.UIInvokeAsync(() =>
                {
                    IsNavigating = true;
                });

                var navigationTask = _templateSelector.GetNavigationTask(target);

                await AvaloniauiExtensions.UIInvokeAsync(() =>
                {
                    Content = target;
                });
                var view = await navigationTask;

                await AvaloniauiExtensions.UIInvokeAsync(() =>
                {
                    IsNavigating = false;
                });

                return;
            }
            catch (Exception)
            {
                await AvaloniauiExtensions.UIInvokeAsync(() =>
                {
                    IsNavigating = false;
                });
                throw;
            }
        }

        protected void SetBindingContent()
        {
            _contentControl.Bind(ContentControl.ContentProperty,
                   new Binding(nameof(Content))
                   {
                       Source = this,
                       Mode = BindingMode.OneWay // 改为单向绑定
                   });
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Task DeActivateAsync(NavigationContext target)
        {
            throw new NotImplementedException();
        }
    }

}
