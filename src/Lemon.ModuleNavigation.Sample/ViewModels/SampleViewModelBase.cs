using ReactiveUI;
using System;

namespace Lemon.ModuleNavigation.Sample.ViewModels;

public class SampleViewModelBase : ReactiveObject, IDisposable
{
    public virtual string Greeting => $"Welcome to {GetType().Name}";
    public virtual void Dispose()
    {
        
    }
}
