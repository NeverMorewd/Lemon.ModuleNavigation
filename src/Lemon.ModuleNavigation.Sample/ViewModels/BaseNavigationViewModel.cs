using Lemon.ModuleNavigation.Abstractions;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;

namespace Lemon.ModuleNavigation.Sample.ViewModels;

public class BaseNavigationViewModel : ReactiveObject, INavigationAware, IDisposable
{
    public virtual string Greeting => $"Welcome to {GetType().Name}[{Environment.ProcessId}][{Environment.CurrentManagedThreadId}]{Environment.NewLine}{DateTime.Now:yyyy-MM-dd HH-mm-ss.ffff}";


    public BaseNavigationViewModel()
    {
        UnloadViewCommand = ReactiveCommand.Create(() =>
        {
            var code = this.GetHashCode();
            Debug.WriteLine(code);
            RequestUnload?.Invoke();
        });
    }
    public ReactiveCommand<Unit, Unit> UnloadViewCommand
    {
        get;
    }

    public event Action? RequestUnload;

    public virtual void Dispose()
    {
        
    }

    public virtual bool IsNavigationTarget(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters is not null)
        {
            if (navigationContext.Parameters.TryGetValue("requestNew", out bool requestNew))
            {
                return !requestNew;
            }
        }
        return true;
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext)
    {
       
    }

    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {
        
    }
}
