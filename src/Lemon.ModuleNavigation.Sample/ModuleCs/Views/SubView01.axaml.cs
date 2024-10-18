using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Sample.ModuleCs.Views;

public partial class SubView01 : UserControl, IView
{
    public SubView01()
    {
        InitializeComponent();
    }

    public void SetDataContext(IViewModel viewModel)
    {
        DataContext = viewModel;
    }
}