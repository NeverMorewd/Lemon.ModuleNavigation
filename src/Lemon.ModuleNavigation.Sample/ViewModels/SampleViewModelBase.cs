using ReactiveUI;
using System;

namespace Lemon.ModuleNavigation.Sample.ViewModels;

public class SampleViewModelBase : ReactiveObject, IDisposable
{
    public virtual string Greeting => $"Welcome to {GetType().Name}[{Environment.ProcessId}][{Environment.CurrentManagedThreadId}]{Environment.NewLine}{DateTime.Now:yyyy-MM-dd HH-mm-ss.ffff}";
    public virtual void Dispose()
    {
        
    }
}
