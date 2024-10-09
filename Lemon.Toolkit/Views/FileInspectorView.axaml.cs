using Avalonia.Controls;
using Lemon.Extensions.ModuleNavigation.Abstracts;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.Toolkit.Views;

[RequiresUnreferencedCode("")]
public partial class FileInspectorView : UserControl, IView
{
    public FileInspectorView()
    {
        InitializeComponent();
    }
    public void SetDataContext(IViewModel viewModel)
    {
        this.DataContext = viewModel;
    }
}