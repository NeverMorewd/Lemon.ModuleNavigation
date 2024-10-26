namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IDialogService
    {
        void Show<TParam,TReturn>(string name, 
            TParam parameters, 
            Action<TReturn> callback);

        void Show<TParam, TReturn>(string 
            name, TParam parameters, 
            Action<TReturn> callback, 
            string windowName);

        void ShowDialog<TParam, TReturn>(string name, 
            TParam parameters, 
            Action<TReturn> callback);

        void ShowDialog<TParam, TReturn>(string name, 
            TParam parameters, 
            Action<TReturn> callback, 
            string windowName);
    }
}
