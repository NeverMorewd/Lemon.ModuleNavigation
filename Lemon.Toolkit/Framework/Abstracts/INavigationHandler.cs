using System;

namespace Lemon.Toolkit.Framework.Abstracts
{
    public interface INavigationHandler<in T>
    {
        void NavigateTo(T target);
    }
}
