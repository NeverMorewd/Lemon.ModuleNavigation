using Lemon.ModuleNavigation.Abstractions;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive;

namespace Lemon.ModuleNavigation.SampleViewModel;

public class BaseNavigationViewModel : ReactiveObject, INavigationAware, ICanUnload
{
    public virtual string Greeting => $"Welcome to {GetType().Name}[{Environment.ProcessId}][{Environment.CurrentManagedThreadId}]{Environment.NewLine}{DateTime.Now:yyyy-MM-dd HH-mm-ss.ffff}";
    public virtual string? Alias => GetType().Name;
    public BaseNavigationViewModel()
    {
        UnloadViewCommand = ReactiveCommand.Create(() =>
        {
            var code = this.GetHashCode();
            Debug.WriteLine(code);
            RequestUnload?.Invoke();
        });
    }
    public ReactiveCommand<Unit,Unit> UnloadViewCommand
    {
        get;
    }

    public event Action? RequestUnload;

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
