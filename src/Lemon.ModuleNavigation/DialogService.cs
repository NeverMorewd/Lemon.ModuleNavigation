using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation
{
    public abstract class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;
        public DialogService(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public void Show<TParam, TReturn>(string viewName, 
            TParam parameters, 
            Action<TReturn> callback)
        {
            var dialogWindow = _serviceProvider.GetRequiredKeyedService<IDialogWindow>("key");
            var dialogViewModel = _serviceProvider.GetRequiredKeyedService<IDialogAware>(viewName);
            dialogWindow.Content = _serviceProvider.GetRequiredKeyedService<IView>(viewName);
            dialogWindow.DataContext = dialogViewModel;
            dialogViewModel.OnDialogOpened(parameters);
            dialogWindow.Closed += (s, e) => 
            {
                dialogViewModel.OnDialogClosed();
            };
            dialogWindow.Show();
        }

        public void Show<TParam, TReturn>(string name, TParam parameters, Action<TReturn> callback, string windowName)
        {
            throw new NotImplementedException();
        }

        public void ShowDialog<TParam, TReturn>(string name, TParam parameters, Action<TReturn> callback)
        {
            throw new NotImplementedException();
        }

        public void ShowDialog<TParam, TReturn>(string name, TParam parameters, Action<TReturn> callback, string windowName)
        {
            throw new NotImplementedException();
        }
    }
}
