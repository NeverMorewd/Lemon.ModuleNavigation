using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions
{
    public static class RegionsExtension
    {
        public static IRegion ToContainer(this Control control) 
        {
            return control switch
            {
                TabControl tabControl => new TabRegion(tabControl),
                ItemsControl itemsControl => new ItemsRegion(itemsControl),
                ContentControl contentControl => new ContentRegion(contentControl),
                _ => throw new NotSupportedException($"Unsupported control:{control.GetType()}"),
            };
        }
    }
}
