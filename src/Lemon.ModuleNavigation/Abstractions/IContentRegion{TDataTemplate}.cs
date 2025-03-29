using Lemon.ModuleNavigation.Abstractions;

namespace Lemon.ModuleNavigation.Regions;

public interface IContentRegion<TDataTemplate> : IRegion
{
    public object? Content
    {
        get;
        set;
    }
    TDataTemplate? RegionTemplate
    {
        get;
        set;
    }
}
