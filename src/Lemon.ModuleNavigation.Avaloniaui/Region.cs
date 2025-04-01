using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Avaloniaui;

public abstract class Region : IRegion
{
    private readonly ConcurrentDictionary<string, IView> _viewCache = new();
    private readonly ConcurrentItem<(IView View, INavigationAware NavigationAware)> _current = new();

    public Region()
    {
        RegionTemplate = CreateRegionDataTemplate();
    }

    public abstract string Name { get; }
    public abstract ObservableCollection<NavigationContext> Contexts { get; }

    public IDataTemplate? RegionTemplate { get; set; }

    public abstract void Activate(NavigationContext target);
    public abstract void DeActivate(string viewName);
    public abstract void DeActivate(NavigationContext target);

    private IDataTemplate CreateRegionDataTemplate()
    {
        return new FuncDataTemplate<NavigationContext>((context, np) =>
        {
            if (context == null)
                return null;

            bool needNewView = context.RequestNew ||
                !_viewCache.TryGetValue(context.ViewName, out IView? view);

            if (needNewView)
            {
                view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.ViewName);
                var navigationAware = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.ViewName);

                if (_current.TryTakeData(out var previousData))
                {
                    previousData.NavigationAware.OnNavigatedFrom(context);
                }
                if (!navigationAware.IsNavigationTarget(context))
                    return null;

                view.DataContext = navigationAware;
                navigationAware.OnNavigatedTo(context);
                _current.SetData((view, navigationAware));
                _viewCache.TryAdd(context.ViewName, view);
            }
            else
            {
                view = _viewCache[context.ViewName];
            }

            return view as Control;
        });
    }
}