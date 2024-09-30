using System;

namespace Lemon.Hosting.Modularization.Abstracts
{
    public interface INavigationService<out T>
    {
        IDisposable OnNavigation(INavigationHandler<T> navigation);
    }
}
