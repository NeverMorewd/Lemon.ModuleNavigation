using Lemon.ModuleNavigation.Abstractions;
using System.Windows;

namespace Lemon.ModuleNavigation.Wpf;

public interface IDialogWindow : IDialogWindowBase
{
    Task<bool?> ShowDialogAsync(Window? owner = null);
    bool? ShowDialog();
}
