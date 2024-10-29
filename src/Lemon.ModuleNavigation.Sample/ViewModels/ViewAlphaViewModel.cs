﻿using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Dialogs;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ViewModels
{
    public class ViewAlphaViewModel : SampleViewModelBase, IDialogAware, INavigationAware
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
    }
}
