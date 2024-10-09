using Avalonia.Controls;
using Lemon.Extensions.ModuleNavigation.Abstracts;

namespace Lemon.Toolkit.Views;

public partial class CompareView : UserControl,IView
{
    public CompareView()
    {
        InitializeComponent();
    }

    public void SetDataContext(IViewModel viewModel)
    {
        this.DataContext = viewModel;
    }
}