using Lemon.Toolkit.Framework.Abstracts;
using ReactiveUI;

namespace Lemon.Toolkit.Framework
{
    public class ViewModelBase : ReactiveObject, IViewModel
    {
        public virtual void Dispose()
        {

        }
    }
}
