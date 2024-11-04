using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Containers
{
    public interface ITabContainer<TDataTemplate> : IRegion
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
        void Add(object item);
    }
}
