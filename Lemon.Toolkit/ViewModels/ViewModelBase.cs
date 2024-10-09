using Lemon.ModuleNavigation.Abstracts;
using ReactiveUI;

namespace Lemon.Toolkit.ViewModels
{
    public class ViewModelBase : ReactiveObject, IViewModel
    {
        public virtual void Dispose()
        {

        }
    }
}
