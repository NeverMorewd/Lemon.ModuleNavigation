using System;

namespace Lemon.Toolkit.Framework
{
    public interface ITabModule
    {
        public string Name
        {
            get;
        }
        public bool LoadDefault
        {
            get;
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
