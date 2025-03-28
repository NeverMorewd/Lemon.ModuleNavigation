using System.Threading.Tasks;
using System.Windows;

namespace Lemon.ModuleNavigation.Wpf.Dialogs;

/// <summary>
/// Interaction logic for DefaultDialogWindow.xaml
/// </summary>
public partial class DefaultDialogWindow : Window, IDialogWindow
{
    public readonly static string Key = nameof(DefaultDialogWindow);
    public DefaultDialogWindow()
    {
        InitializeComponent();
    }

    public async Task<bool?> ShowDialogAsync(Window? owner = null)
    {
        TaskCompletionSource<bool?> taskCompletionSource = new();
        if (Owner != null)
        {
            Owner = owner;
        }
        var result = ShowDialog();
        taskCompletionSource.SetResult(result);
        return await taskCompletionSource.Task;
    }
}
