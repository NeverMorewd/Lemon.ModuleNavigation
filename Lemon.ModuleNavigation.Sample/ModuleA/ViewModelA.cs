using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;

namespace Lemon.ModuleNavigation.Sample.ModuleAs
{
    public class ViewModelA : ViewModelBase, IViewModel
    {
        public override string Greeting => $"{base.Greeting}:Load immediately";
    }
}
