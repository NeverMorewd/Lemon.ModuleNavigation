using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleCs.ViewModels
{
    public class SubViewModel01 : ViewModelBase, IViewModel
    {
        public SubViewModel01(IServiceProvider serviceProvider)
        {
            // this code will raise stackoverflow
            //var navigationContext = serviceProvider.GetRequiredService<NavigationContext>();
        }
    }
}
