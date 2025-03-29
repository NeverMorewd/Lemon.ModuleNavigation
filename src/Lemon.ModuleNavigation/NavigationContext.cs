using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation;

public class NavigationContext
{
    private readonly Guid _guid;
    internal NavigationContext(string viewName, 
        string regionName,
        IServiceProvider serviceProvider,
        bool requestNew,
        NavigationParameters? navigationParameters)
    {
        TargetViewName = viewName;
        Parameters = navigationParameters;
        RequestNew = requestNew;
        RegionName = regionName;
        _guid = Guid.NewGuid();
        ServiceProvider = serviceProvider;
    }
    public static ViewNameComparer ViewNameComparer => new();
    public static StrictComparer StrictComparer => new();
    public string TargetViewName 
    { 
        get; 
        private set; 
    }
    public NavigationParameters? Parameters 
    { 
        get; 
        private set; 
    }
    public bool RequestNew
    {
        get;
        private set;
    }
    public string RegionName
    { 
        get; 
        private set; 
    }
    public Uri? Uri
    {
        get;
        set;
    }
    public Guid Guid => _guid;
    public IServiceProvider ServiceProvider
    {
        get;
    }
    public override string ToString()
    {
        return TargetViewName;
    }
}
public class ViewNameComparer : IEqualityComparer<NavigationContext>
{
    public bool Equals(NavigationContext? x, NavigationContext? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.TargetViewName == y.TargetViewName;
    }

    public int GetHashCode(NavigationContext obj)
    {
        return obj.TargetViewName.GetHashCode();
    }
}
public class StrictComparer : IEqualityComparer<NavigationContext>
{
    public bool Equals(NavigationContext? x, NavigationContext? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.Guid == y.Guid;
    }

    public int GetHashCode(NavigationContext obj)
    {
        return obj.Guid.GetHashCode();
    }
}
