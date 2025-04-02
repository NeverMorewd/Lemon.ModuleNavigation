using System.ComponentModel;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IItemsRegionDataContext<TDataTemplate> : INotifyPropertyChanged
{
    object? SelectedItem
    {
        get;
        set;
    }
    TDataTemplate? ItemTemplate
    {
        get;
        set;
    }
}
