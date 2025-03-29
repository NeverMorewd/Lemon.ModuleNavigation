namespace Lemon.ModuleNavigation.Abstractions;

public interface IDialogWindowBase
{
    string? Title { get; set; }
    object? Content { get; set; }
    object? DataContext { get; set; }
    event EventHandler Closed;
    void Close();
    void Show();
}
