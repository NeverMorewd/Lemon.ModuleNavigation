using System;

namespace Lemon.Toolkit.Framework.Abstracts
{
    public interface INavigationService<out T>
    {
        IDisposable OnNavigation(INavigationHandler<T> navigation);
    }
}
