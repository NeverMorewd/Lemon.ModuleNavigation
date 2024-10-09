using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Sample;

public partial class ViewA : UserControl,IView
{
    public ViewA()
    {
        InitializeComponent();
    }

    public void SetDataContext(IViewModel viewModel)
    {
        DataContext = viewModel;
    }
}