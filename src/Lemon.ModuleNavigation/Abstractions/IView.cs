namespace Lemon.ModuleNavigation.Abstractions;

public interface IView
{
    object? DataContext
    {
        get;
        set;
    }
}
