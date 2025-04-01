using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ViewModels
{
    public class ViewAlphaViewModel : BaseNavigationViewModel, IDialogAware
    {
        private readonly ILogger _logger;
        public ViewAlphaViewModel(ILogger<ViewAlphaViewModel> logger)
        {
            _logger = logger;
            CloseCommand = ReactiveCommand.Create(() => 
            {
                var param = new DialogParameters
                {
                    { "from", nameof(ViewAlphaViewModel) }
                };
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
            });
        }
        private bool _isDialog = false;
        public bool IsDialog
        {
            get => _isDialog;
            set
            {
                this.RaiseAndSetIfChanged(ref _isDialog, value);
            }
        }
        public ReactiveCommand<Unit, Unit> CloseCommand { get; } 
        public string Title => nameof(ViewAlphaViewModel);
        public event Action<IDialogResult>? RequestClose;

        public void OnDialogClosed()
        {
            _logger.LogInformation("OnDialogClosed");
        }

        public void OnDialogOpened(IDialogParameters? parameters)
        {
            _logger.LogInformation($"OnDialogOpened:{parameters?.ToString()}");
            IsDialog = true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
