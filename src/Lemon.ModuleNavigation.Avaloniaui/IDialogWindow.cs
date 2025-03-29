using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstractions;

namespace Lemon.ModuleNavigation.Avaloniaui;

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
