using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Dialogs;
using ReactiveUI;
using Splat;
using System.Diagnostics;
using System.Reactive;

namespace Lemon.ModuleNavigation.WpfSample;

public class MainWindowViewModel : BaseViewModel, IServiceAware
{
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    public MainWindowViewModel(INavigationService navigationService, 
        IDialogService dialogService, 
        IServiceProvider serviceProvider)
    {
        _dialogService = dialogService;
        _navigationService = navigationService;
        _navigationService.RequestViewNavigation("ContentRegion", "ViewAlpha", false);
        ServiceProvider = serviceProvider;
        NavigateToViewCommand = ReactiveCommand.Create<string>(content => 
        {
            var viewName = content;
            var requestNew = false;
            if (content.EndsWith(".RequestNew"))
            {
                viewName = content.Replace(".RequestNew", string.Empty);
                requestNew = true;

            }
            _navigationService.RequestViewNavigation("ContentRegion", viewName, requestNew);
            _navigationService.RequestViewNavigation("TabRegion", viewName, requestNew);
            _navigationService.RequestViewNavigation("ItemsRegion", viewName, requestNew);
        });

        ShowCommand = ReactiveCommand.Create<string>(content =>
        {
            var param = new DialogParameters
            {
                { "parent", nameof(MainWindowViewModel) }
            };
            _dialogService.Show(content, null, param, result =>
            {
                Debug.WriteLine($"Call back:{result}");
            });
            Debug.WriteLine($"Show over!");
        });

        ShowDialogCommand = ReactiveCommand.CreateFromTask<string>(async content =>
        {
            var param = new DialogParameters
            {
                { "parent", nameof(MainWindowViewModel) }
            };
            await _dialogService.ShowDialog(content,
                null,
                param,
                result =>
                {
                    Debug.WriteLine($"Call back:{result}");
                });
            Debug.WriteLine($"ShowDialog over!");
        });

        ShowDialogSyncCommand = ReactiveCommand.Create<string>(content =>
        {
            var param = new DialogParameters
            {
                { "parent", nameof(MainWindowViewModel) }
            };
            var result = _dialogService.WaitShowDialog(content,
                null,
                param,
                result =>
                {
                    Debug.WriteLine($"Call back:{result}");
                });
            Debug.WriteLine($"ShowDialog over:{result}");
        });

    }

    public IServiceProvider ServiceProvider
    {
        get;
    }

    public ReactiveCommand<string, Unit> NavigateToViewCommand
    {
        get;
    }
    public ReactiveCommand<string, Unit> ShowCommand
    {
        get;
    }
    public ReactiveCommand<string, Unit> ShowDialogCommand
    {
        get;
    }
    public ReactiveCommand<string, Unit> ShowDialogSyncCommand
    {
        get;
    }
}
