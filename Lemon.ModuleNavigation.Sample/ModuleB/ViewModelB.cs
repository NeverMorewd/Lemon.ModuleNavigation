using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;

namespace Lemon.ModuleNavigation.Sample.ModuleB
{
    public class ViewModelB : ViewModelBase, IViewModel
    {
        public string Greeting => "Welcome to ViewModelB!";
        public void Dispose()
        {
            
        }
    }
}
