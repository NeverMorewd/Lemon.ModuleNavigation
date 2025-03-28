using Avalonia.Controls;

namespace Lemon.ModuleNavigation.Avaloniaui.Dialogs;

public partial class DefaultDialogWindow : Window, IDialogWindow
{
    public readonly static string Key = nameof(DefaultDialogWindow);
    public DefaultDialogWindow()
    {
        InitializeComponent();
    }
}