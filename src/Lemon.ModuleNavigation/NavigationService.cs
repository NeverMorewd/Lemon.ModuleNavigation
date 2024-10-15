using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation
{
    public class NavigationService : INavigationService<IModule>
    {
        private readonly List<INavigationHandler> _handlers = [];
        public NavigationService()
        {
            
        }

        public IDisposable OnNavigation(INavigationHandler<IModule> moduleHandler)
        {
            _handlers.Add(moduleHandler);
            return new Cleanup(_handlers, moduleHandler);
        }
        public IDisposable OnNavigation(INavigationHandler handler)
        {
            _handlers.Add(handler);
            return new Cleanup(_handlers, handler);
        }
        public void NavigateTo(IModule module)
        {
            foreach (var handler in _handlers)
            {
                if (handler is INavigationHandler<IModule> moduleHandler)
                {
                    moduleHandler.OnNavigateTo(module);
                }
            }
        }
        public void NavigateTo(string moduleKey)
        {
            foreach (var handler in _handlers)
            {
                handler.OnNavigateTo(moduleKey);
            }
        }

        private class Cleanup(List<INavigationHandler> handlers, INavigationHandler handler)
            : IDisposable
        {
            public void Dispose()
            {
                handlers?.Remove(handler);
            }
        }
    }

}
