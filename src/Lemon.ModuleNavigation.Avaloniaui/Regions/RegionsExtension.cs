using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions
{
    public static class RegionsExtension
    {
        public static IRegion ToContainer(this Control control, string name) 
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
}
