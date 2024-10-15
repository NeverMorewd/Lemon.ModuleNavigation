using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation
{
    public class NavigationService : INavigationService<IModule>
    {
        private readonly List<INavigationHandler> _handlers = [];
        /// <summary>
        /// NavigateTo with module
        /// </summary>
        /// <param name="module"></param>
        public void NavigateTo(IModule module)
        {
            HandlerTraverse(module);
        }
        /// <summary>
        /// NavigateTo with moduleKey
        /// </summary>
        /// <param name="moduleKey"></param>
        public void NavigateTo(string moduleKey)
        {
            HandlerTraverse(moduleKey);
        }

        IDisposable INavigationService<IModule>.OnNavigation(INavigationHandler<IModule> moduleHandler)
        {
            _handlers.Add(moduleHandler);
            return new Cleanup(_handlers, moduleHandler);
        }
        IDisposable INavigationService.OnNavigation(INavigationHandler handler)
        {
            _handlers.Add(handler);
            return new Cleanup(_handlers, handler);
        }

        private void HandlerTraverse(IModule module)
        {
            foreach (var handler in _handlers)
            {
                if (handler is INavigationHandler<IModule> moduleHandler)
                {
                    moduleHandler.OnNavigateTo(module);
                }
            }
        }
        private void HandlerTraverse(string moduleKey)
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
