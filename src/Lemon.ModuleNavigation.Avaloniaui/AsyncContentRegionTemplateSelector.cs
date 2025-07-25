// File: AsyncContentRegionTemplateSelector.cs
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Lemon.ModuleNavigation.Avaloniaui;

public class AsyncContentRegionTemplateSelector : IDataTemplate
{
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = [];
    private readonly ConcurrentItem<AsyncViewNavigationPair> Current = new();
    private readonly ConcurrentDictionary<int, AsyncViewNavigationPair> ViewCache = new();
    private readonly ConcurrentDictionary<int, Lazy<Task<IView>>> NavigationTasks = new();
    private readonly ConcurrentDictionary<int, TaskCompletionSource<IView>> NavigationCompletionSources = new();
    private readonly IAsyncRegion _region;
    private readonly TimeSpan _navigationTimeout = TimeSpan.FromSeconds(30);

    public AsyncContentRegionTemplateSelector(IAsyncRegion region)
    {
        _region = region;
    }

    public Control? Build(object? param)
    {
        if (param is not NavigationContext context)
            return null;

        return BuildWithPlaceholder(context);
    }

    public bool Match(object? data) => data is NavigationContext;

    public Task<IView> GetNavigationTask(NavigationContext context)
    {
        if (NavigationCompletionSources.TryGetValue(context.ViewKey, out var existingSource))
            return existingSource.Task;

        var source = new TaskCompletionSource<IView>();
        NavigationCompletionSources[context.ViewKey] = source;

        // 添加超时保护
        var cts = new CancellationTokenSource(_navigationTimeout);
        cts.Token.Register(() => source.TrySetCanceled());

        return source.Task;
    }

    private Control BuildWithPlaceholder(NavigationContext context)
    {
        var container = new ContentControl
        {
            Content = CreateLoadingIndicator()
        };
        _ = ResolveViewAsync(context, container);
        return container;
    }

    private Control CreateLoadingIndicator() => new TextBlock
    {
        Text = "Loading...",
        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
    };

    private Control CreateErrorIndicator(string errorMessage) => new StackPanel
    {
        Children =
        {
            new TextBlock
            {
                Text = "Navigation Error",
                FontWeight = Avalonia.Media.FontWeight.Bold,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            },
            new TextBlock
            {
                Text = errorMessage,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            }
        }
    };

    private async Task ResolveViewAsync(NavigationContext context, ContentControl container)
    {
        try
        {
            var task = NavigationTasks.GetOrAdd(context.ViewKey, _ => new Lazy<Task<IView>>(() => PerformNavigationAsync(context))).Value;
            var view = await task;

            try
            {
                await AvaloniauiExtensions.UIInvokeAsync(() => ReplaceContentSafely(container, view));
                context.View = view;
            }
            catch (Exception uiEx)
            {
                Debug.WriteLine($"[UI Error] {uiEx.Message}");
            }

            if (NavigationCompletionSources.TryGetValue(context.ViewKey, out var completionSource))
                completionSource.TrySetResult(view);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Navigation Error] ViewKey: {context.ViewKey}, Message: {ex.Message}");
            await AvaloniauiExtensions.UIInvokeAsync(() => container.Content = CreateErrorIndicator(ex.Message));

            if (NavigationCompletionSources.TryGetValue(context.ViewKey, out var completionSource))
                completionSource.TrySetException(ex);
        }
        finally
        {
            NavigationTasks.TryRemove(context.ViewKey, out _);
            NavigationCompletionSources.TryRemove(context.ViewKey, out _);
        }
    }

    private async Task<IView> PerformNavigationAsync(NavigationContext context)
    {
        var reused = await TryReuseCachedViewAsync(context);
        if (reused != null) return reused;

        return await CreateAndActivateViewAsync(context);
    }

    private async Task<IView?> TryReuseCachedViewAsync(NavigationContext context)
    {
        if (!ViewCache.TryGetValue(context.ViewKey, out var cached))
            return null;

        var canReuse = await cached.NavigationAware.IsNavigationTargetAsync(context);
        if (!canReuse) return null;

        if (Current.TryTakeData(out var last))
            await last!.OnNavigatedFromAsync(context);

        await cached.OnNavigatedToAsync(context);
        Current.SetData(cached);
        return cached.View;
    }

    private async Task<IView> CreateAndActivateViewAsync(NavigationContext context)
    {
        var view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.ViewName);
        var navigationAware = context.ServiceProvider.GetRequiredKeyedService<IAsyncNavigationAware>(context.ViewName);
        var pair = new AsyncViewNavigationPair(view, navigationAware);

        if (Current.TryTakeData(out var previous))
            await previous!.OnNavigatedFromAsync(context);

        view.DataContext = pair.NavigationAware;
        await pair.NavigationAware.OnNavigatedToAsync(context);
        context.Alias = pair.NavigationAware.Alias;

        if (pair.NavigationAware is IAsyncCanUnload canUnload)
            canUnload.RequestUnloadAsync += () => DeactivateAsync(context);

        Current.SetData(pair);
        ViewCache.AddOrUpdate(context.ViewKey, pair, (_, _) => pair);

        return view;
    }

    private async Task DeactivateAsync(NavigationContext context)
    {
        if (Current.TryTakeData(out var current))
            await current!.OnNavigatedFromAsync(context);

        ViewCache.TryRemove(context.ViewKey, out _);
        await _region.DeActivateAsync(context);
    }

    private void ReplaceContentSafely(ContentControl container, IView view)
    {
        if (view is Control control && control.Parent is Visual parent)
        {
            if (parent is Panel panel)
                panel.Children.Remove(control);
            else if (parent is ContentControl cc)
                cc.Content = null;
        }

        container.Content = view;
    }
}
