using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstractions;

namespace Lemon.ModuleNavigation.Avaloniaui;

public static class RegionsExtension
{
    public static IRegion ToRegion(this Control control, string name) 
    {
        return control switch
        {
            TabControl tabControl => new TabRegion(tabControl, name),
            ItemsControl itemsControl => new ItemsRegion(itemsControl, name),
            ContentControl contentControl => new ContentRegion(contentControl, name),
            _ => throw new NotSupportedException($"Unsupported control:{control.GetType()}"),
        };
    }
}
