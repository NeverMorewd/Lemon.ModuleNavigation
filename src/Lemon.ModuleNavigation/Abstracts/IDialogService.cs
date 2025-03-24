namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IDialogService
    {
        void Show(string name, 
            string? windowName = null,
            IDialogParameters? parameters = null,
            Action<IDialogResult>? callback = null);

        Task ShowDialog(string name,
            string? windowName = null,
            IDialogParameters? parameters = null,
            Action<IDialogResult>? callback = null);

        IDialogResult WaitShowDialog(string name,
            string? windowName = null,
            IDialogParameters? parameters = null,
            Action<IDialogResult>? callback = null);
    }
}
