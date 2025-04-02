using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive;

namespace Lemon.ModuleNavigation.WpfSample;

public class MainWindowViewModel : ReactiveObject, IServiceAware
{
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly IRegionManager _regionManager;
    public MainWindowViewModel(INavigationService navigationService,
        IDialogService dialogService,
        IRegionManager regionManager,
        IServiceProvider serviceProvider)
    {
        _dialogService = dialogService;
        _regionManager = regionManager;
        _navigationService = navigationService;
        _navigationService.RequestViewNavigation("ContentRegion", "ViewAlpha");
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
            _navigationService.RequestViewNavigation("ContentRegion", viewName, new NavigationParameters { { "requestNew", requestNew } });
            _navigationService.RequestViewNavigation("TabRegion", viewName, new NavigationParameters { { "requestNew", requestNew } });
            _navigationService.RequestViewNavigation("ItemsRegion", viewName, new NavigationParameters { { "requestNew", requestNew } });
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
        UnloadViewCommand = ReactiveCommand.Create<NavigationContext>((context) =>
        {
            _regionManager.RequestViewUnload(context);
        });
    }

    public IServiceProvider ServiceProvider
    {
        get;
    }
    public ReactiveCommand<NavigationContext, Unit> UnloadViewCommand
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
