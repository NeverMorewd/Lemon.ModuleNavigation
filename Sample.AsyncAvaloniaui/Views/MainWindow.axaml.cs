using Avalonia;
using Avalonia.Controls;
using System.Diagnostics;

namespace Sample.AsyncAvaloniaui.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        if (Debugger.IsAttached)
        {
            this.AttachDevTools();
        }
    }
}