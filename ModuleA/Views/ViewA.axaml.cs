using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lemon.ModuleNavigation.Abstracts;

namespace ModuleA;

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