namespace Lemon.ModuleNavigation.Core
{
    public class NavigationContext
    {
        public readonly static NavigationContextComparer Comparer = new();
        public NavigationContext(string targetName,
            string containerName)
            : this(targetName, 
                  containerName, 
                  false, 
                  null)
        {

        }
        public NavigationContext(string targetName, 
            string containerName,
            bool requestNew,
            NavigationParameters? navigationParameters)
        {
            TargetName = targetName;
            Parameters = navigationParameters;
            RequestNew = requestNew;
            ContainerName = containerName;
        }
        public string TargetName 
        { 
            get; 
            private set; 
        }
        public NavigationParameters? Parameters 
        { 
            get; 
            private set; 
        }
        public bool RequestNew
        {
            get;
            private set;
        }
        public string ContainerName
        { 
            get; 
            private set; 
        }
        public Uri? Uri
        {
            get;
            set;
        }
        public override string ToString()
        {
            return TargetName;
        }
    }
    public class NavigationContextComparer : IEqualityComparer<NavigationContext>
    {
        public bool Equals(NavigationContext? x, NavigationContext? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.TargetName == y.TargetName;
        }

        public int GetHashCode(NavigationContext obj)
        {
            return obj.TargetName.GetHashCode();
        }
    }
}
