using Lemon.ModuleNavigation.Abstracts;

namespace ModuleA.ViewModels
{
    public class ViewModelB : IViewModel
    {
        public void Dispose()
        {
            
        }
        public string Greeting => nameof(ViewModelB);
    }
}
