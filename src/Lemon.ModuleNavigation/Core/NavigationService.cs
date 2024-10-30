﻿using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Core
{
    public class NavigationService : IModuleNavigationService<IModule>, IViewNavigationService
    {
        private readonly List<IModuleNavigationHandler> _handlers = [];
        private readonly List<IViewNavigationHandler> _viewHandlers = [];

        public void NavigateTo(IModule module)
        {
            foreach (var handler in _handlers)
            {
                if (handler is IModuleNavigationHandler<IModule> moduleHandler)
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

        IDisposable IModuleNavigationService<IModule>.BindingNavigationHandler(IModuleNavigationHandler<IModule> moduleHandler)
        {
            _handlers.Add(moduleHandler);
            return new Cleanup<IModuleNavigationHandler>(_handlers, moduleHandler);
        }
        IDisposable IModuleNavigationService.BindingNavigationHandler(IModuleNavigationHandler handler)
        {
            _handlers.Add(handler);
            return new Cleanup<IModuleNavigationHandler>(_handlers, handler);
        }

        public void NavigateToView(string containerKey, 
            string viewKey, 
            bool requestNew = false)
        {
            foreach (var handler in _viewHandlers)
            {
                handler.OnNavigateTo(containerKey, viewKey, requestNew);
            }
        }

        IDisposable IViewNavigationService.BindingViewNavigationHandler(IViewNavigationHandler handler)
        {
            _viewHandlers.Add(handler);
            return new Cleanup<IViewNavigationHandler>(_viewHandlers, handler);
        }

        public void NavigateToView(string containerKey, 
            string viewKey, 
            NavigationParameters parameters, 
            bool requestNew = false)
        {
            foreach (var handler in _viewHandlers)
            {
                handler.OnNavigateTo(containerKey, viewKey, requestNew);
            }
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