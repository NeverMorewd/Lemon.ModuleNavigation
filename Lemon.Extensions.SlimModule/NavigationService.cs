using Lemon.Extensions.SlimModule.Abstracts;

namespace Lemon.Extensions.SlimModule
{
    public class NavigationService : INavigationService<IModule>
    {
        public List<INavigationHandler<IModule>> _handlers = [];
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
        private class Cleanup : IDisposable
        {
            private readonly List<INavigationHandler<IModule>> _handlers;
            private readonly INavigationHandler<IModule> _handler;
            public Cleanup(List<INavigationHandler<IModule>> handlers
            , INavigationHandler<IModule> handler)
            {
                _handler = handler;
                _handlers = handlers;
            }

            public void Dispose()
            {
                _handlers?.Remove(_handler);
            }
        }
    }

}
