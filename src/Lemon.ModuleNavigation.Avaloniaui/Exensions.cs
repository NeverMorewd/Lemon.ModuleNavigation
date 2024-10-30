using Avalonia.Controls;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public static class Exensions
    {
        public static bool ContainsItem<T>(this ItemCollection items,
                                           T item,
                                           IEqualityComparer<T> comparer) where T : class
        {
            return items.Cast<T>().Any(x => comparer.Equals(x, item));
        }
    }
}
