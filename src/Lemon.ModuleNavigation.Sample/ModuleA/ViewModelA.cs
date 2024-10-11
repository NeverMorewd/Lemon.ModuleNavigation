using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;
using System;

namespace Lemon.ModuleNavigation.Sample.ModuleAs
{
    public class ViewModelA : ViewModelBase, IViewModel
    {
        public override string Greeting => $"{base.Greeting}:Load immediately{Environment.NewLine}{DateTime.Now}";
    }
}
