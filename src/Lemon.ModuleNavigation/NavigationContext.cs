using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lemon.ModuleNavigation;

public class NavigationContext : INotifyPropertyChanged
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
        NavigationParameters? navigationParameters)
    {
        ViewName = viewName;
        Parameters = navigationParameters;
        RegionName = regionName;
        ServiceProvider = serviceProvider;
    }
    public static ViewNameComparer ViewNameComparer => new();
    public static StrictComparer StrictComparer => new();
    public string ViewName
    {
        get;
        private set;
    }
    private string? _alias;
    public string? Alias
    {
        get => _alias;
        set
        {
            _alias = value;
            OnPropertyChanged();
        }
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

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
