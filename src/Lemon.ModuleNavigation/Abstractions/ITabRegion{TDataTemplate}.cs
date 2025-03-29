using Lemon.ModuleNavigation.Abstractions;

namespace Lemon.ModuleNavigation.Regions;

public interface ITabRegion<TDataTemplate> : IRegion
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
    void Add(object item);
}
