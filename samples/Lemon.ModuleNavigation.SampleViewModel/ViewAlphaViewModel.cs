using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using ReactiveUI;
using System.Reactive;

namespace Lemon.ModuleNavigation.SampleViewModel;

public class ViewAlphaViewModel : BaseNavigationViewModel, IDialogAware
{
    public ViewAlphaViewModel()
    {
    }

    public override string? Alias => "AlphaView";
    public string Title => nameof(ViewAlphaViewModel);

    public event Action<IDialogResult>? RequestClose;
    public ReactiveCommand<Unit, Unit> CloseCommand => ReactiveCommand.Create(() =>
    {
        var param = new DialogParameters
        {
            { "from", nameof(ViewAlphaViewModel) }
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

    public override bool IsNavigationTarget(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters is not null)
        {
            if (navigationContext.Parameters.TryGetValue("requestNew", out bool requestNew))
            {
                return !requestNew;
            }
        }
        return true;
    }
}
