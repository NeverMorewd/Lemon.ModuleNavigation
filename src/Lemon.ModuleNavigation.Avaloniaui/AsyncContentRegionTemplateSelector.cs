using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Avaloniaui;

public class AsyncContentRegionTemplateSelector : IDataTemplate
{
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = [];
    private readonly ConcurrentItem<AsyncViewNavigationPair> Current = new();
    private readonly ConcurrentDictionary<int, AsyncViewNavigationPair> ViewCache = new();
    private readonly ConcurrentDictionary<int, Task<IView>> NavigationTasks = new();
    private readonly ConcurrentDictionary<int, TaskCompletionSource<IView>> NavigationCompletionSources = new();
    private readonly IAsyncRegion _region;

    public AsyncContentRegionTemplateSelector(IAsyncRegion region)
    {
        _region = region;
    }

    public Control? Build(object? param)
    {
        if (param is not NavigationContext context)
            throw new ArgumentException("Expected NavigationContext", nameof(param));

        return BuildWithPlaceholder(context);
    }

    public bool Match(object? data) => data is NavigationContext;

    public Task<IView> GetNavigationTask(NavigationContext context)
    {
        if (NavigationTasks.TryGetValue(context.ViewKey, out var existingTask))
        {
            return existingTask;
        }

        var completionSource = new TaskCompletionSource<IView>();
        NavigationCompletionSources.TryAdd(context.ViewKey, completionSource);

        return completionSource.Task;
    }

    private Control BuildWithPlaceholder(NavigationContext context)
    {
        return AvaloniauiExtensions.UIInvoke(() => 
        {
            var container = new ContentControl
            {
                Content = CreateLoadingIndicator()
            };

            _ = ResolveViewAsync(context, container);
            return container;
        });
    }

    private Control CreateLoadingIndicator()
    {
        return new TextBlock
        {
            Text = "Loading...",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };
    }

    private async ValueTask ResolveViewAsync(NavigationContext context, ContentControl container)
    {
        try
        {
            if (NavigationTasks.TryGetValue(context.ViewKey, out var existingTask))
            {
                var existingView = await existingTask;
                await AvaloniauiExtensions.UIInvokeAsync(() =>
                {
                    container.Content = existingView;
                    context.View = existingView;
                });
                return;
            }

            var navigationTask = PerformNavigationAsync(context);
            NavigationTasks.TryAdd(context.ViewKey, navigationTask);

            var view = await navigationTask;

            await AvaloniauiExtensions.UIInvokeAsync(() =>
            {
                //container.Content = null;
                container.Content = view;
                context.View = view;
            });
            if (NavigationCompletionSources.TryGetValue(context.ViewKey, out var completionSource))
            {
                completionSource.SetResult(view);
            }
        }
        catch (Exception ex)
        {
            await AvaloniauiExtensions.UIInvokeAsync(() =>
            {
                container.Content = CreateErrorIndicator(ex.Message);
            });
            if (NavigationCompletionSources.TryGetValue(context.ViewKey, out var completionSource))
            {
                completionSource.SetException(ex);
            }
        }
        finally
        {
            NavigationTasks.TryRemove(context.ViewKey, out _);
            NavigationCompletionSources.TryRemove(context.ViewKey, out _);
        }
    }

    private async Task<IView> PerformNavigationAsync(NavigationContext context)
    {
        if (ViewCache.TryGetValue(context.ViewKey, out var cache))
        {
            var canReuse = await cache.NavigationAware.IsNavigationTargetAsync(context);
            if (canReuse)
            {
                //await _region.DeActivateAsync(context);
                return cache.View;
            }
        }

        var view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.ViewName);
        var navigationAware = context.ServiceProvider.GetRequiredKeyedService<IAsyncNavigationAware>(context.ViewName);
        var pair = new AsyncViewNavigationPair(view, navigationAware);

        if (Current.TryTakeData(out var previous))
        {
            await previous!.OnNavigatedFromAsync(context);
        }

        view.DataContext = pair.NavigationAware;
        await pair.NavigationAware.OnNavigatedToAsync(context);

        context.Alias = pair.NavigationAware.Alias;

        if (pair.NavigationAware is IAsyncCanUnload canUnload)
        {
            canUnload.RequestUnloadAsync += () => DeactivateAsync(context);
        }

        Current.SetData(pair);
        ViewCache.AddOrUpdate(context.ViewKey, pair, (key, existing) => existing);

        return view;
    }

    private Control CreateErrorIndicator(string errorMessage)
    {
        return new StackPanel
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
    }

    private async Task DeactivateAsync(NavigationContext context)
    {
        if (Current.TryTakeData(out var current))
        {
            await current!.OnNavigatedFromAsync(context);
        }

        ViewCache.TryRemove(context.ViewKey, out _);
        await _region.DeActivateAsync(context);
    }
}