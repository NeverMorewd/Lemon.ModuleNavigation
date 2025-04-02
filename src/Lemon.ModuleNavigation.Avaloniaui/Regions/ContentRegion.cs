using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;
using Lemon.ModuleNavigation.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions;

public class ContentRegion : Region, IContentRegionContext<IDataTemplate>
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

    private IDataTemplate? _contentTemplate;
    public IDataTemplate? ContentTemplate
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
        _contentControl.Bind(ContentControl.ContentTemplateProperty,
               new Binding(nameof(ContentTemplate))
               {
                   Source = this,
                   Mode = BindingMode.TwoWay
               });
    }
    protected virtual void SetBindingContent()
    {
        _contentControl.Bind(ContentControl.ContentProperty,
               new Binding(nameof(Content))
               {
                   Source = this,
                   Mode = BindingMode.TwoWay
               });
    }
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
