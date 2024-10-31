using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Containers
{
    public interface IItemsContainer<TDataTemplate> : INavigationContainer
    {
        object? SelectedItem
        {
            get;
            set;
        }
        TDataTemplate? ContainerTemplate
        {
            get;
            set;
        }
        void ScrollIntoView(int index);
        void ScrollIntoView(object item);
        void Add(object item);
    }
}
