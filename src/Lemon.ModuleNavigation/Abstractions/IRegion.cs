using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Abstractions;

public interface IRegion
{
    string Name { get; }
    ObservableCollection<NavigationContext> Contexts 
    { 
        get;
    }
    void Activate(NavigationContext target);
    void DeActivate(NavigationContext target);
    void DeActivate(string viewName);

    ///ongoing: last view
}
