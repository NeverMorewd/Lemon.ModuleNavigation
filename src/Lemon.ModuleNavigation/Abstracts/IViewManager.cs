namespace Lemon.ModuleNavigation.Abstracts
{
    public interface IViewManager : IObservable<IView>
    {
        IEnumerable<IView> Views { get; }
    }
}
