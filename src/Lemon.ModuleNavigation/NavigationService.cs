using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Lemon.ModuleNavigation.Internal;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation;

public class NavigationService : INavigationService
{
    private readonly List<IModuleNavigationHandler> _handlers = [];
    private readonly List<IViewNavigationHandler> _viewHandlers = [];

    // reserve only one for now.
    private readonly ConcurrentStack<(IModule module, NavigationParameters parameter)> _bufferModule = [];
    private readonly ConcurrentStack<(string moduleName, NavigationParameters parameter)> _bufferModuleName = [];
    private readonly ConcurrentStack<(string regionName, string viewName, NavigationParameters? parameter, bool requestNew)> _bufferViewName = [];

    public NavigationService()
    {

    }

    public void RequestModuleNavigate(IModule module, NavigationParameters parameters)
    {
        foreach (var handler in _handlers)
        {
            if (handler is IModuleNavigationHandler<IModule> moduleHandler)
            {
                moduleHandler.OnNavigateTo(module, parameters);
            }
        }
        _bufferModule.Push((module, parameters));
    }
    public void RequestModuleNavigate(string moduleName, NavigationParameters parameters)
    {
        foreach (var handler in _handlers)
        {
            handler.OnNavigateTo(moduleName, parameters);
        }
        _bufferModuleName.Push((moduleName, parameters));
    }
    public void RequestViewNavigation(string regionName,
        string viewKey,
        bool requestNew = false)
    {
        foreach (var handler in _viewHandlers)
        {
            handler.OnNavigateTo(regionName, viewKey, requestNew);
        }
        _bufferViewName.Push((regionName, viewKey, null, requestNew));
    }
    public void RequestViewNavigation(string regionName,
        string viewKey,
        NavigationParameters parameters,
        bool requestNew = false)
    {
        foreach (var handler in _viewHandlers)
        {
            handler.OnNavigateTo(regionName, viewKey, parameters, requestNew);
        }
        _bufferViewName.Push((regionName, viewKey, parameters, requestNew));
    }
    IDisposable IModuleNavigationService<IModule>.BindingNavigationHandler(IModuleNavigationHandler<IModule> moduleHandler)
    {
        _handlers.Add(moduleHandler);
        if (_bufferModule.TryPop(out var item))
        {
            moduleHandler.OnNavigateTo(item.module, item.parameter);
            _bufferModule.Clear();
        }
        return new DisposableAction(() =>
        {
            _handlers.Remove(moduleHandler);
        });
    }
    IDisposable IModuleNavigationService.BindingNavigationHandler(IModuleNavigationHandler handler)
    {
        _handlers.Add(handler);
        if (_bufferModuleName.TryPop(out var item))
        {
            handler.OnNavigateTo(item.moduleName, item.parameter);
            _bufferModuleName.Clear();
        }
        return new DisposableAction(() =>
        {
            _handlers.Remove(handler);
        });
    }

    IDisposable IViewNavigationService.BindingViewNavigationHandler(IViewNavigationHandler handler)
    {
        _viewHandlers.Add(handler);
        foreach (var (regionName, viewName, parameters, requestNew) in _bufferViewName)
        {
            if (parameters == null)
            {
                handler.OnNavigateTo(regionName, viewName, requestNew);
            }
            else
            {
                handler.OnNavigateTo(regionName, viewName, parameters, requestNew);
            }
        }
        return new DisposableAction(() =>
        {
            _viewHandlers.Remove(handler);
        });
    }
}
