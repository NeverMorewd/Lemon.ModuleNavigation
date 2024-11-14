using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lemon.ModuleNavigation.Avaloniaui.Dialogs;

namespace Lemon.ModuleNavigation.Sample;

public partial class CustomDialogWindow : Window, IAvaDialogWindow
{
    public CustomDialogWindow()
    {
        InitializeComponent();
    }
}