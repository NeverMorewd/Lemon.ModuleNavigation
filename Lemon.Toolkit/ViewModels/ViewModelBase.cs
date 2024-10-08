using Lemon.Extensions.SlimModule.Abstracts;
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
