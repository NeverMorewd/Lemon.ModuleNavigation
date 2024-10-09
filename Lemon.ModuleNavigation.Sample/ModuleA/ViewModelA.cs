using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Sample.ViewModels;

namespace Lemon.ModuleNavigation.Sample.ModuleA
{
    public class ViewModelA : ViewModelBase, IViewModel
    {
        public string Greeting => "Welcome to ViewModelA!";
        public void Dispose()
        {
            
        }
    }
}
