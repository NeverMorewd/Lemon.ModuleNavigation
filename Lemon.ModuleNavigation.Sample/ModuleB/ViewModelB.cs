using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleBs
{
    public class ViewModelB : ViewModelBase, IViewModel
    {
        public override string Greeting => $"{base.Greeting}:{Environment.NewLine}{DateTime.Now}";
    }
}
