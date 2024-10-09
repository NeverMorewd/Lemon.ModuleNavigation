using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation
{
    public class NavigationService : INavigationService<IModule>
    {
        private readonly List<INavigationHandler<IModule>> _handlers = [];
        public NavigationService()
        {
            
        }

        public IDisposable OnNavigation(INavigationHandler<IModule> handler)
        {
            _handlers.Add(handler);
            return new Cleanup(_handlers, handler);
        }
        public void NavigateTo(IModule module)
        {
            foreach (var service in _handlers)
            {
                service.NavigateTo(module);
            }
        }
        private class Cleanup(List<INavigationHandler<IModule>> handlers, INavigationHandler<IModule> handler)
            : IDisposable
        {
            public void Dispose()
            {
                handlers?.Remove(handler);
            }
        }
    }

}
