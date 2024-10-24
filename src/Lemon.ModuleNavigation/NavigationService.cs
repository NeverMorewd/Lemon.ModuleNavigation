using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation
{
    public class NavigationService : INavigationService<IModule>, IViewNavigationService
    {
        private readonly List<INavigationHandler> _handlers = [];
        private readonly List<IViewNavigationHandler> _viewHandlers = [];

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

        IDisposable INavigationService<IModule>.BindingNavigationHandler(INavigationHandler<IModule> moduleHandler)
        {
            _handlers.Add(moduleHandler);
            return new Cleanup<INavigationHandler>(_handlers, moduleHandler);
        }
        IDisposable INavigationService.BindingNavigationHandler(INavigationHandler handler)
        {
            _handlers.Add(handler);
            return new Cleanup<INavigationHandler>(_handlers, handler);
        }

        public void NavigateToView(string containerKey, string viewKey)
        {
            foreach (var handler in _viewHandlers)
            {
                handler.OnNavigateTo(containerKey, viewKey);
            }
        }

        public void NavigateToView<TView>(string containerKey) where TView : notnull
        {
            foreach (var handler in _viewHandlers)
            {
                handler.OnNavigateTo<TView>(containerKey);
            }
        }
        IDisposable IViewNavigationService.BindingViewNavigationHandler(IViewNavigationHandler handler)
        {
            _viewHandlers.Add(handler);
            return new Cleanup<IViewNavigationHandler>(_viewHandlers, handler);
        }


        private class Cleanup<T>(List<T> handlers, T handler)
            : IDisposable
        {
            public void Dispose()
            {
                handlers?.Remove(handler);
            }
        }
    }

}
