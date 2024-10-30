namespace Lemon.ModuleNavigation.Internals
{
    public sealed record ViewProperty
    {
        public ViewProperty(string containerName, string viewName, bool requestNew)
        {
            ContainerName = containerName;
            ViewName = viewName;
            RequestNew = requestNew;
        }
        public string ContainerName
        {
            get;
            private set;
        }
        public string ViewName
        {
            get;
            private set;
        }
        public bool RequestNew
        {
            get;
            private set;
        }

    }
}
