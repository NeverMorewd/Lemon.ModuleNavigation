using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lemon.Toolkit.Framework.Abstracts;
using Lemon.Toolkit.ViewModels;
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