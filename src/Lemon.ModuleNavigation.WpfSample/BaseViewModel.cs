using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using ReactiveUI;

namespace Lemon.ModuleNavigation.WpfSample;

public class BaseViewModel : ReactiveObject, INavigationAware
{
    public virtual string Greeting => $"Welcome to {GetType().Name}[{Environment.ProcessId}][{Environment.CurrentManagedThreadId}]{Environment.NewLine}{DateTime.Now:yyyy-MM-dd HH-mm-ss.ffff}";
    public virtual bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext)
    {
        //throw new NotImplementedException();
    }

    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {
       //throw new NotImplementedException();
    }
}
