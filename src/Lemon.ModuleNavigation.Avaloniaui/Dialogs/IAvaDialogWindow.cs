using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui.Dialogs;

public interface IDialogWindow : IDialogWindowBase
{
    Task<TResult> ShowDialog<TResult>(Window owner);
    Task ShowDialog(Window owner);
}

[Obsolete("Use IDialogWindow instead, they are equivalent.")]
public interface IAvaDialogWindow : IDialogWindowBase
{
    Task<TResult> ShowDialog<TResult>(Window owner);
    Task ShowDialog(Window owner);
}
