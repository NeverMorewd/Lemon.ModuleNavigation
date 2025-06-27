using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Lemon.ModuleNavigation.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions;

public class ContentRegion : Region, IContentRegionContext<IDataTemplate>
{
    private readonly ContentControl _contentControl;
    private readonly IDataTemplate _templateSelector;
    public ContentRegion(string name, ContentControl contentControl) : base(name)
    {
        _contentControl = contentControl;
        _templateSelector = new ContentRegionTemplateSelector(this);
        _contentControl.DataTemplates.Add(_templateSelector);
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

    IDataTemplate? IContentRegionContext<IDataTemplate>.ContentTemplate
    {
        get => _templateSelector;
        set
        {
            //readonly
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public override void Activate(NavigationContext target)
    {
        Content = target;
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

    //protected virtual void SetBindingContentTemplate()
    //{
    //    ContentTemplate = RegionContentTemplate;
    //    _contentControl.Bind(ContentControl.ContentTemplateProperty,
    //           new Binding(nameof(ContentTemplate))
    //           {
    //               Source = this,
    //               Mode = BindingMode.TwoWay
    //           });
    //}
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
