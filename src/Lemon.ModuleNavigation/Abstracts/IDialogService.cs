namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IDialogService
    {
        void Show(string name, 
            IDialogParameters? parameters = null, 
            Action<IDialogResult>? callback = null);

        void Show(string name, 
            string windowName,
            IDialogParameters? parameters = null,
            Action<IDialogResult>? callback = null);

        Task ShowDialog(string name,
            IDialogParameters? parameters = null, 
            Action<IDialogResult>? callback = null);

        Task ShowDialog(string name,
            string windowName,
            IDialogParameters? parameters = null,
            Action<IDialogResult>? callback = null);
    }
}
