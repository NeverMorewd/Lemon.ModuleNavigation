using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Dialogs;
using ReactiveUI;
using System.Reactive;

namespace Lemon.ModuleNavigation.WpfSample.ViewModels;

public class ViewBetaViewModel : BaseViewModel, IDialogAware
{
    public string Title => nameof(ViewBetaViewModel);

    public event Action<IDialogResult>? RequestClose;
    public ReactiveCommand<Unit, Unit> CloseCommand => ReactiveCommand.Create(() => 
    {
        var param = new DialogParameters
        {
            { "from", nameof(ViewBetaViewModel) }
        };
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
    });

    private bool _isDialog = false;
    public bool IsDialog
    {
        get => _isDialog;
        set
        {
            this.RaiseAndSetIfChanged(ref _isDialog, value);
        }
    }

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters? parameters)
    {
        IsDialog = true;
    }
}
