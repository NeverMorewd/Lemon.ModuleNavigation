using Lemon.ModuleNavigation.Abstractions;
using ReactiveUI;
using System;

namespace Lemon.ModuleNavigation.Sample.ViewModels;

public class BaseNavigationViewModel : ReactiveObject, INavigationAware, IDisposable
{
    public virtual string Greeting => $"Welcome to {GetType().Name}[{Environment.ProcessId}][{Environment.CurrentManagedThreadId}]{Environment.NewLine}{DateTime.Now:yyyy-MM-dd HH-mm-ss.ffff}";

    public event Action? RequestUnload;

    public virtual void Dispose()
    {
        
    }

    public virtual bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return !navigationContext.RequestNew;
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext)
    {
       
    }

    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {
        
    }
}
