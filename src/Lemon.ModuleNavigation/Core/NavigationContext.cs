using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Core
{
    public class NavigationContext
    {
        public NavigationContext(INavigationService navigationService, 
            Uri uri)
            : this(navigationService, 
                  uri , 
                  null)
        {

        }
        public NavigationContext(INavigationService navigationService, 
            Uri uri, 
            NavigationParameters? navigationParameters)
        {
            NavigationService = navigationService;
            Uri = uri;
            Parameters = navigationParameters;
        }
        public INavigationService NavigationService 
        { 
            get; 
            private set; 
        }
        public Uri Uri 
        { 
            get; 
            private set; 
        }
        public NavigationParameters? Parameters 
        { 
            get; 
            private set; 
        }
    }
}
