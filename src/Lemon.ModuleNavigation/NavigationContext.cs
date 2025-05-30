using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;

namespace Lemon.ModuleNavigation;

public class NavigationContext
{
    [Obsolete("requestNew was obsolete.Consider IsNavigationTarget() in INavigationAware instead.")]
    internal NavigationContext(string viewName, 
        string regionName,
        IServiceProvider serviceProvider,
        bool requestNew,
        NavigationParameters? navigationParameters)
    {
        ViewName = viewName;
        Parameters = navigationParameters;
        RequestNew = requestNew;
        RegionName = regionName;
        ServiceProvider = serviceProvider;
    }
    internal NavigationContext(string viewName,
        string regionName,
        IServiceProvider serviceProvider,
        NavigationParameters? navigationParameters,
        string? alias)
    {
        ViewName = viewName;
        Parameters = navigationParameters;
        RegionName = regionName;
        ServiceProvider = serviceProvider;
        Alias = alias;
    }
    public static ViewNameComparer ViewNameComparer => new();
    public static StrictComparer StrictComparer => new();
    public string ViewName 
    { 
        get; 
        private set; 
    }
    public string? Alias
    {
        get;
        private set;
    }
    public NavigationParameters? Parameters 
    { 
        get; 
        private set; 
    }
    [Obsolete("requestNew was obsolete.Consider IsNavigationTarget() in INavigationAware instead.")]
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
    public int Key => GetHashCode();
    public IServiceProvider ServiceProvider
    {
        get;
    }
    public IView? View
    {
        get;
        set;
    }
    public override string ToString()
    {
        return $"{RegionName}.{ViewName}";
    }
}
public class ViewNameComparer : IEqualityComparer<NavigationContext>
{
    public bool Equals(NavigationContext? x, NavigationContext? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.ViewName == y.ViewName;
    }

    public int GetHashCode(NavigationContext obj)
    {
        return obj.ViewName.GetHashCode();
    }
}
public class StrictComparer : IEqualityComparer<NavigationContext>
{
    public bool Equals(NavigationContext? x, NavigationContext? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.Key == y.Key;
    }

    public int GetHashCode(NavigationContext obj)
    {
        return obj.Key.GetHashCode();
    }
}
