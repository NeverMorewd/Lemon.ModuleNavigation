namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IDialogAware
    {
        string Title { get; }
        event Action<IDialogResult>? RequestClose;
        void OnDialogClosed();
        void OnDialogOpened(IDialogParameters? parameters);
    }
}
