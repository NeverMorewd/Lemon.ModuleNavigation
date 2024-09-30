using System;

namespace Lemon.Toolkit.Framework.Abstracts
{
    public interface IModule
    {
        public string Name
        {
            get;
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
        public void Initialize();

        public Type ViewType
        {
            get;
        }
        public Type ViewModelType
        {
            get;
        }
    }
}
