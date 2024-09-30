using Lemon.Hosting.Modularization;
using Lemon.Toolkit.ViewModels;
using System;

namespace Lemon.Toolkit.Modules
{
    public class HomeModule : Module<HomeView, HomeViewModel>
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
