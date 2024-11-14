using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui.Dialogs
{
    public class AvaDialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;
        public AvaDialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show(string name,
            IDialogParameters? parameters = null,
            Action<IDialogResult>? callback = null)
        {
            ShowCore(name, null, false, parameters, callback).Wait();
        }

        public void Show(string name,
            string windowName,
            IDialogParameters? parameters = null, 
            Action<IDialogResult>? callback = null)
        {
            ShowCore(name, windowName, false, parameters, callback).Wait();
        }

        public async Task ShowDialog(string name,
            IDialogParameters? parameters = null, 
            Action<IDialogResult>? callback = null)
        {
            await ShowCore(name, null, true, parameters, callback);
        }

        public async Task ShowDialog(string name, 
            string windowName,
            IDialogParameters? parameters = null,
            Action<IDialogResult>? callback = null)
        {
            await ShowCore(name, windowName, true, parameters, callback);
        }

        private async Task ShowCore(string name,
            string? windowName,
            bool showDialog,
            IDialogParameters? parameters = null,
            Action<IDialogResult>? callback = null)
        {
            IAvaDialogWindow dialogWindow;
            if (string.IsNullOrEmpty(windowName))
            {
                dialogWindow = _serviceProvider.GetRequiredKeyedService<IAvaDialogWindow>(DefaultDialogWindow.Key);
            }
            else
            {
                dialogWindow = _serviceProvider.GetRequiredKeyedService<IAvaDialogWindow>(windowName);
            }
            
            var dialogViewModel = _serviceProvider.GetRequiredKeyedService<IDialogAware>(name);
            dialogWindow.Title = dialogViewModel.Title;
            dialogWindow.Content = _serviceProvider.GetRequiredKeyedService<IView>(name);
            dialogWindow.DataContext = dialogViewModel;
            dialogViewModel.OnDialogOpened(parameters);
            dialogViewModel.RequestClose += (result) =>
            {
                callback?.Invoke(result);
                dialogWindow.Close();
            };
            dialogWindow.Closed += (s, e) =>
            {
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
}
