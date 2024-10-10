using ReactiveUI;
using System;

namespace Lemon.ModuleNavigation.Sample.ViewModels;

public class ViewModelBase : ReactiveObject, IDisposable
{
    public virtual string Greeting => $"Welcome to {this.GetType().Name}";
    public virtual void Dispose()
    {
        
    }
}
