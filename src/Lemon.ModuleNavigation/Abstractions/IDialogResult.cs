using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IDialogResult
{
    /// <summary>
    /// The parameters from the dialog.
    /// </summary>
    IDialogParameters Parameters { get; }

    /// <summary>
    /// The result of the dialog.
    /// </summary>
    ButtonResult Result { get; }
}
