using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Containers
{
    public interface IContentContainer<TDataTemplate> : IRegion
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
