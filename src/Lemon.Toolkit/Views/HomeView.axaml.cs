using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;
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
        DataContext = viewModel;
    }
}