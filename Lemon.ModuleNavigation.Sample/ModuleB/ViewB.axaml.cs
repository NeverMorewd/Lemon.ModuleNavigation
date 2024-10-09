using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Sample;

public partial class ViewB : UserControl, IView
{
    public ViewB()
    {
        InitializeComponent();
    }

    public void SetDataContext(IViewModel viewModel)
    {
        DataContext = viewModel;
    }
}