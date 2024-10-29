using Avalonia;
using Avalonia.Controls;
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
            ShowCore(name, null, false, parameters, callback);
        }

        public void Show(string name,
            string windowName,
            IDialogParameters? parameters = null, 
            Action<IDialogResult>? callback = null)
        {
            ShowCore(name, windowName, false, parameters, callback);
        }

        public void ShowDialog(string name,
            IDialogParameters? parameters = null, 
            Action<IDialogResult>? callback = null)
        {
            ShowCore(name, null, true, parameters, callback);
        }

        public void ShowDialog(string name, 
            string windowName,
            IDialogParameters? parameters = null,
            Action<IDialogResult>? callback = null)
        {
            ShowCore(name, windowName, true, parameters, callback);
        }

        private void ShowCore(string name,
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
                dialogWindow.Close();
                callback?.Invoke(result);
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
                    // todo
                    dialogWindow.ShowDialog(owner);
                }
            }
            else
            {
                dialogWindow.Show();
            }
        }
    }
}
