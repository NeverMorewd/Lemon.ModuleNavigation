using Lemon.Toolkit.Framework;
using Lemon.Toolkit.ViewModels;
using System;

namespace Lemon.Toolkit.Modules
{
    public class HomeModule : TabModule<HomeView, HomeViewModel>
    {
        public HomeModule(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }
        public override bool LoadDefault
        {
            get => true;
        }
    }
}
