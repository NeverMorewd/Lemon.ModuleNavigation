using Lemon.ModuleNavigation.Abstracts;
using System.Windows;

namespace Lemon.ModuleNavigation.Wpf.Dialogs;

public interface IDialogWindow : IDialogWindowBase
{
    Task<bool?> ShowDialogAsync(Window? owner = null);
    bool? ShowDialog();
}
