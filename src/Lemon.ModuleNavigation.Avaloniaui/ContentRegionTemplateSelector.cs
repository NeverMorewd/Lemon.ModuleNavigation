using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Avaloniaui;

public class ContentRegionTemplateSelector : IDataTemplate
{
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = [];
    private readonly ConcurrentItem<ViewNavigationPair> Current = new();
    private readonly ConcurrentDictionary<int, ViewNavigationPair> ViewCache = new();
    private readonly IRegion _region;

    public ContentRegionTemplateSelector(IRegion region)
    {
        _region = region;
    }

    public Control? Build(object? param)
    {
        if (param is not NavigationContext context)
            throw new ArgumentException("Expected NavigationContext", nameof(param));

        context.View ??= ResolveView(context);
        return context.View as Control;
    }

    public bool Match(object? data) => data is NavigationContext;

    private IView ResolveView(NavigationContext context)
    {
        if (ViewCache.TryGetValue(context.ViewKey, out var cache) 
            && cache.NavigationAware.IsNavigationTarget(context))
        {
            ///https://github.com/AvaloniaUI/Avalonia/issues/19129
            return cache.View;
        }
        var view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.ViewName);
        var navigationAware = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.ViewName);

        if (Current.TryTakeData(out var previous))
        {
            previous.OnNavigatedFrom(context);
        }

        view.DataContext = navigationAware;
        navigationAware.OnNavigatedTo(context);
        context.Alias = navigationAware.Alias;

        if (navigationAware is ICanUnload canUnload)
        {
            canUnload.RequestUnload += () => DeActivate(context);
        }
        Current.SetData(new(view, navigationAware));
        ViewCache.AddOrUpdate(context.ViewKey, 
            new ViewNavigationPair(view,navigationAware), 
            (key, existing) => existing);

        return view;
    }

    private void DeActivate(NavigationContext context)
    {
        ViewCache.TryRemove(context.ViewKey, out _);
        _region.DeActivate(context);
    }
}
