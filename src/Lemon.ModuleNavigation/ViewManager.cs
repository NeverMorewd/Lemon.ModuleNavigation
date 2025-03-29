using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation;

public static class ViewManager
{
    static ViewManager()
    {
        InternalViewDiscriptions = [];
    }
    internal static ConcurrentDictionary<string, ViewDiscription> InternalViewDiscriptions
    {
        get;
    }
    public static bool TryGetViewDiscription(string key, out ViewDiscription viewDiscription)
    {
        if (ViewDiscriptions.TryGetValue(key, out var view))
        {
            viewDiscription = new ViewDiscription
            {
                ViewKey = key,
                ViewType = view.ViewType,
                ViewModelType = view.ViewModelType,
                ViewClassName = view.ViewType.Name
            };
            return true;
        }
        viewDiscription = default;
        return false;
    }
    public static IReadOnlyDictionary<string, ViewDiscription> ViewDiscriptions
    {
        get
        {
#if NET8_0_OR_GREATER
            return InternalViewDiscriptions.AsReadOnly();
#else
            return InternalViewDiscriptions;
#endif
        }
    }
    public static IReadOnlyList<string> Keys
    {
        get
        {
            return InternalViewDiscriptions.Keys.ToList().AsReadOnly();
        }
    }
    public static IReadOnlyList<Type> ViewTypes
    {
        get
        {
            return InternalViewDiscriptions.Values.Select(v => v.ViewType).ToList().AsReadOnly();
        }
    }
    public static IReadOnlyList<Type> ViewModelTypes
    {
        get
        {
            return InternalViewDiscriptions.Values.Select(v => v.ViewModelType).ToList().AsReadOnly();
        }
    }
}
