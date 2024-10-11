using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleCs
{
    public class ViewModelC : ViewModelBase, IViewModel
    {
        public override string Greeting => $"{base.Greeting}:{Environment.NewLine}{DateTime.Now}";
    }
}
