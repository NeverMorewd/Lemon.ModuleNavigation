using Lemon.ModuleNavigation.Abstractions;
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

    /// <summary>
    /// When Views with same ViewName were found, the latest one will be picked.
    /// </summary>
    /// <param name="target"></param>
    public override void Activate(NavigationContext target)
    {
        if (ViewCache.TryGetValue(target, out IView? accurateView))
        {
            target.View = accurateView;
            Content = target;
        }
        else if (ViewNameCache.TryGetValue(target.ViewName, out IView? view)
            && view.DataContext is INavigationAware navigationAware
            && navigationAware.IsNavigationTarget(target))
        {
            var context = Contexts.First(c => c.ViewName == target.ViewName);
            context.View = view;
            Content = context;
        }
        else
        {
            Contexts.Add(target);
            Content = target;
        }
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
    
    protected virtual void SetBindingContentTemplate()
    {
        ContentTemplate = RegionContentTemplate;
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
