using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Avaloniaui.Regions;

namespace Lemon.ModuleNavigation.Avaloniaui;

public static class RegionsExtension
{
    public static IRegion ToRegion(this Control control, string name) 
    {
        return control switch
        {
            TabControl tabControl => new TabRegion(name, tabControl),
            ItemsControl itemsControl => new ItemsRegion(name, itemsControl),
            ContentControl contentControl => new ContentRegion(name, contentControl),
            _ => throw new NotSupportedException($"Unsupported control:{control.GetType()}"),
        };
    }

    public static IAsyncRegion ToAsyncRegion(this Control control, string name)
    {
        return control switch
        {
            //TabControl tabControl => new TabRegion(name, tabControl),
            //ItemsControl itemsControl => new ItemsRegion(name, itemsControl),
            ContentControl contentControl => new AsyncContentRegion(name, contentControl),
            _ => throw new NotSupportedException($"Unsupported control:{control.GetType()}"),
        };
    }
}
