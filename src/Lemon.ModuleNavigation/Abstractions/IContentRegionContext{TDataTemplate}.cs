using System.ComponentModel;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IContentRegionContext<TDataTemplate> : INotifyPropertyChanged
{
    object? Content { get; set; }
    TDataTemplate? ContentTemplate { get; set; }
}

