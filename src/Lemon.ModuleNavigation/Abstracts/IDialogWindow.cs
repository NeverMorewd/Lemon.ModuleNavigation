using System.ComponentModel;

namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IDialogWindow
    {
        object Content { get; set; }
        object DataContext { get; set; }
        event EventHandler Closed;
        event CancelEventHandler Closing;
        void Close();
        void Show();
        bool? ShowDialog();
    }
}
