namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IDialogAware
    {
        string Title { get; }
        event Action<object> RequestClose;
        void OnDialogClosed();
        void OnDialogOpened<TParam>(TParam parameters);
    }
}
