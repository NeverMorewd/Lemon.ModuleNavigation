using System;

namespace Lemon.Hosting.Modularization.Abstracts
{
    public interface INavigationHandler<in T>
    {
        void NavigateTo(T target);
    }
}
