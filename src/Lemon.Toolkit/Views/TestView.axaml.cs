using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.Toolkit;

public partial class TestView : UserControl, IView
{
    public TestView()
    {
        InitializeComponent();
    }

    public void SetDataContext(IViewModel viewModel)
    {
        DataContext = viewModel;
    }
}