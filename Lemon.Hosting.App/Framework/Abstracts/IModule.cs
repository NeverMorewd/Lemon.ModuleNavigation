namespace Lemon.Hosting.Modularization.Abstracts
{
    public interface IModule
    {
        public string Key
        {
            get;
        }
        public string? Alias
        {
            get;
            set;
        }
        public bool LoadOnDemand
        {
            get;
        }
        public bool IsInitialized
        {
            get;
        }
        public bool IsActivated
        {
            get;
            set;
        }
        public Type ViewType
        {
            get;
        }
        public Type ViewModelType
        {
            get;
        }
        public void Initialize();
    }
}
