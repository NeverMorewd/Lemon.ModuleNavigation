using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui;

public class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;
    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Show(string name,
        string? windowName = null,
        IDialogParameters? parameters = null, 
        Action<IDialogResult>? callback = null)
    {
        ShowCore(name, windowName, false, parameters, callback).Wait();
    }

    public async Task ShowDialog(string name, 
        string? windowName = null,
        IDialogParameters? parameters = null,
        Action<IDialogResult>? callback = null)
    {
        await ShowCore(name, windowName, true, parameters, callback);
    }

    public IDialogResult WaitShowDialog(string name, 
        string? windowName = null, 
        IDialogParameters? parameters = null)
    {
       return ShowDialogCoreSync(name, windowName, parameters);
    }


    private IDialogResult ShowDialogCoreSync(string name,
                            string? windowName,
                            IDialogParameters? parameters = null,
                            Action<IDialogResult>? callback = null)
    {
        var tcs = new TaskCompletionSource<IDialogResult>();
        var task = ShowCore(name,
            windowName,
            true,
            parameters,
            result =>
            {
                callback?.Invoke(result);
                tcs.TrySetResult(result);
            });
        return tcs.Task.WaitOnDispatcherFrame();
    }

    private async Task ShowCore(string name,
        string? windowName,
        bool showDialog,
        IDialogParameters? parameters = null,
        Action<IDialogResult>? callback = null)
    {
        IDialogWindow dialogWindow;
        if (string.IsNullOrEmpty(windowName))
        {
            dialogWindow = _serviceProvider.GetRequiredKeyedService<IDialogWindow>(DefaultDialogWindow.Key);
        }
        else
        {
            dialogWindow = _serviceProvider.GetRequiredKeyedService<IDialogWindow>(windowName);
        }
        
        var dialogViewModel = _serviceProvider.GetRequiredKeyedService<IDialogAware>(name);
        dialogWindow.Title = dialogViewModel.Title;
        dialogWindow.Content = _serviceProvider.GetRequiredKeyedService<IView>(name);
        dialogWindow.DataContext = dialogViewModel;
        dialogViewModel.OnDialogOpened(parameters);

        bool handled = false;
        void RequestCloseHandler(IDialogResult dialogResult)
        {
            callback?.Invoke(dialogResult);
            handled = true;
            dialogWindow.Close();
        }

        dialogViewModel.RequestClose += RequestCloseHandler;
        dialogWindow.Closed += (s, e) =>
        {
            dialogViewModel.RequestClose -= RequestCloseHandler;
            if (!handled)
            {
                callback?.Invoke(new DialogResult(ButtonResult.None));
            }
            dialogViewModel.OnDialogClosed();
        };
        if (showDialog)
        {
            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktop)
            {
                var owner = classicDesktop.MainWindow!;
                await dialogWindow.ShowDialog(owner);
            }
        }
        else
        {
            dialogWindow.Show();
        }
    }
}
