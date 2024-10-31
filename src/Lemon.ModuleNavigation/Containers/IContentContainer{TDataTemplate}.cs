using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Containers
{
    public interface IContentContainer<TDataTemplate> : INavigationContainer
    {
        public object? Content
        {
            get;
            set;
        }
        TDataTemplate? ContainerTemplate
        {
            get;
            set;
        }
    }
}
