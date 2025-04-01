using Lemon.ModuleNavigation.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Lemon.ModuleNavigation.Wpf.Regions;

public class ContentRegion : Region, IContentRegionContext<DataTemplate>
{
    private readonly ContentControl _contentControl;
    public ContentRegion(string name, ContentControl contentControl) : base(name)
    {
        _contentControl = contentControl;
        SetBindingContentTemplate();
        SetBindingContent();
    }

    private object? _content;
    public object? Content
    {
        get => _content;
        set
        {
            _content = value;
            OnPropertyChanged();
        }
    }

    private DataTemplate? _contentTemplate;
    public DataTemplate? ContentTemplate
    {
        get => _contentTemplate;
        set
        {
            _contentTemplate = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public override void Activate(NavigationContext target)
    {
        Content = target;
        Contexts.Add(target);
    }

    public override void DeActivate(string regionName)
    {
        if (Content is NavigationContext current)
        {
            if (current.ViewName == regionName)
            {
                Contexts.Remove(current);
                Content = null;
            }
        }
    }
    public override void DeActivate(NavigationContext navigationContext)
    {
        if (Content is NavigationContext current)
        {
            if (NavigationContext.ViewNameComparer.Equals(current, navigationContext))
            {
                Contexts.Remove(current);
                Content = null;
            }
        }
    }

    protected override IView? ResolveView(NavigationContext context)
    {
        bool needNewView = !ViewNameCache.TryGetValue(context.ViewName, out IView? view);

        if (!needNewView)
        {
            if (view!.DataContext is INavigationAware navigationAware
                && !navigationAware.IsNavigationTarget(context))
            {
                needNewView = true;
            }
        }

        if (needNewView)
        {
            view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.ViewName);
            var navigationAware = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.ViewName);

            if (Current.TryTakeData(out var previousData))
            {
                previousData.NavigationAware.OnNavigatedFrom(context);
            }

            view.DataContext = navigationAware;
            navigationAware.OnNavigatedTo(context);
            navigationAware.RequestUnload += () =>
            {
                DeActivate(context);
            };
            Current.SetData((view, navigationAware));
            ViewNameCache[context.ViewName] = view;
        }

        return view;
    }
    protected virtual void SetBindingContentTemplate()
    {
        ContentTemplate = RegionTemplate;
        BindingOperations.SetBinding(_contentControl,
            ContentControl.ContentTemplateProperty,
            new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(ContentTemplate)),
            });
    }
    protected virtual void SetBindingContent()
    {
        BindingOperations.SetBinding(_contentControl,
            ContentControl.ContentProperty,
            new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(Content)),
                Mode = BindingMode.TwoWay
            });
    }
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
