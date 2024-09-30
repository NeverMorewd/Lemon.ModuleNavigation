using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lemon.Toolkit.Framework.Abstracts;

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