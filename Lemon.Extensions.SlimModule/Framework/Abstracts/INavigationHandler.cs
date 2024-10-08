using System;

namespace Lemon.Extensions.SlimModule.Abstracts
{
    public interface INavigationHandler<in T>
    {
        void NavigateTo(T target);
    }
}
