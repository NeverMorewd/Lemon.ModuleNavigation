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
    private readonly ConcurrentStack<(IModule module, NavigationParameters? parameter)> _bufferModule = [];
    private readonly ConcurrentStack<(string moduleName, NavigationParameters? parameter)> _bufferModuleName = [];
    private readonly ConcurrentStack<(string regionName, string viewName, NavigationParameters? parameter)> _bufferViewName = [];

    public NavigationService()
    {

    }

    public void RequestModuleNavigate(IModule module, NavigationParameters? parameters)
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
    public void RequestModuleNavigate(string moduleName, NavigationParameters? parameters)
    {
        foreach (var handler in _handlers)
        {
            handler.OnNavigateTo(moduleName, parameters);
        }
        _bufferModuleName.Push((moduleName, parameters));
    }
    public void RequestViewNavigation(string regionName,
       string viewName)
    {
        foreach (var handler in _viewHandlers)
        {
            handler.OnNavigateTo(regionName, viewName);
        }
        _bufferViewName.Push((regionName, viewName, null));
    }
    public void RequestViewNavigation(string regionName,
        string viewName,
        NavigationParameters parameters)
    {
        foreach (var handler in _viewHandlers)
        {
            handler.OnNavigateTo(regionName, viewName, parameters);
        }
        _bufferViewName.Push((regionName, viewName, parameters));
    }
    [Obsolete("requestNew was obsolete.Consider IsNavigationTarget() in INavigationAware instead.")]
    public void RequestViewNavigation(string regionName,
        string viewName,
        bool requestNew)
    {
        foreach (var handler in _viewHandlers)
        {
            handler.OnNavigateTo(regionName, viewName);
        }
        _bufferViewName.Push((regionName, viewName, null));
    }
    [Obsolete("requestNew was obsolete.Consider IsNavigationTarget() in INavigationAware instead.")]
    public void RequestViewNavigation(string regionName,
        string viewName,
        NavigationParameters parameters,
        bool requestNew)
    {
        foreach (var handler in _viewHandlers)
        {
            handler.OnNavigateTo(regionName, viewName, parameters);
        }
        _bufferViewName.Push((regionName, viewName, parameters));
    }

    public void RequestViewUnload(string regionName, string viewName)
    {
        foreach (var handler in _viewHandlers)
        {
            handler.OnViewUnload(regionName, viewName);
        }
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

    IDisposable IViewNavigationService.RegisterNavigationHandler(IViewNavigationHandler handler)
    {
        _viewHandlers.Add(handler);
        foreach (var (regionName, viewName, parameters) in _bufferViewName)
        {
            if (parameters == null)
            {
                handler.OnNavigateTo(regionName, viewName);
            }
            else
            {
                handler.OnNavigateTo(regionName, viewName, parameters);
            }
        }
        return new DisposableAction(() =>
        {
            _viewHandlers.Remove(handler);
        });
    }
}
