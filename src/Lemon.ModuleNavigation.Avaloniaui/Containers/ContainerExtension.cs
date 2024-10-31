using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui.Containers
{
    public static class ContainerExtension
    {
        public static INavigationContainer ToContainer(this Control control) 
        {
            return control switch
            {
                TabControl tabControl => new TabContainer(tabControl),
                ItemsControl itemsControl => new ItemsContainer(itemsControl),
                ContentControl contentControl => new ContentContainer(contentControl),
                _ => throw new NotSupportedException($"Unsupported control:{control.GetType()}"),
            };
        }
    }
}
