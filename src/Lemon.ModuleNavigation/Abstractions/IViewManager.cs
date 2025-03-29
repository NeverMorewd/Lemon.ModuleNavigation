namespace Lemon.ModuleNavigation.Abstractions;

public interface IViewManager : IObservable<IView>
{
    IEnumerable<IView> Views { get; }
}
