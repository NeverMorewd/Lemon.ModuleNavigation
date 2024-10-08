using System;

namespace Lemon.Extensions.SlimModule.Abstracts
{
    public interface INavigationService<out T>
    {
        IDisposable OnNavigation(INavigationHandler<T> navigation);
    }
}
