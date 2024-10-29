using Avalonia.Controls;
using Avalonia.Interactivity;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui.Dialogs
{
    public interface IAvaDialogWindow : IDialogWindow
    {
        //event EventHandler<WindowClosingEventArgs>? Closing;
        //event EventHandler<RoutedEventArgs>? Loaded;
        Task<TResult> ShowDialog<TResult>(Window owner);
        Task ShowDialog(Window owner);
    }
}
