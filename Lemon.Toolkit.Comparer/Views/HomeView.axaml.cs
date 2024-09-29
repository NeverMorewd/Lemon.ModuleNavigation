using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lemon.Toolkit.Framework;
using Lemon.Toolkit.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.Toolkit;

[RequiresUnreferencedCode("")]
public partial class HomeView : UserControl, IView
{
    public HomeView()
    {
        InitializeComponent();
    }

    public void SetDataContext(IViewModel viewModel)
    {
        this.DataContext = viewModel;
    }
}