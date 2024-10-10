using Lemon.ModuleNavigation;
using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.Toolkit.ViewModels;
using System;

namespace Lemon.Toolkit.Modules
{
    public class HomeModule : AvaModule<HomeView, HomeViewModel>
    {
        public HomeModule(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }
        public override bool LoadOnDemand
        {
            get => false;
        }
    }
}
