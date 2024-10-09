using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;

namespace ModuleB;

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