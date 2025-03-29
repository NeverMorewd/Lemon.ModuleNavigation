using Lemon.ModuleNavigation.Abstractions;

namespace Lemon.ModuleNavigation.Regions;

public interface IItemsRegion<TDataTemplate> : IRegion
{
    object? SelectedItem
    {
        get;
        set;
    }
    TDataTemplate? RegionTemplate
    {
        get;
        set;
    }
    void ScrollIntoView(int index);
    void ScrollIntoView(object item);
    void Add(object item);
}
