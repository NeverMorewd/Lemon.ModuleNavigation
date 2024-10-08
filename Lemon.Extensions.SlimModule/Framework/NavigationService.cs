using Lemon.Extensions.SlimModule.Abstracts;
using System;
using System.Collections.Generic;

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
            return new Unsubscribe(_handlers, handler);
        }
        public void NavigateTo(IModule module)
        {
            foreach (var service in _handlers)
            {
                service.NavigateTo(module);
            }
        }
        private class Unsubscribe : IDisposable
        {
            private readonly List<INavigationHandler<IModule>> _handlers;
            private readonly INavigationHandler<IModule> _handler;
            public Unsubscribe(List<INavigationHandler<IModule>> handlers
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
