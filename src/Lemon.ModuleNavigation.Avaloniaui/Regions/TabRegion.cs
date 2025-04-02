using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;
using Lemon.ModuleNavigation.Abstractions;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions;

public class TabRegion : ItemsRegion, IContentRegionContext<IDataTemplate>
{
    private readonly TabControl _tabControl;
    public TabRegion(string name, TabControl tabControl) : base(name, tabControl)
    {
        _tabControl = tabControl;
        SetBindingContentTemplate();
    }

    public object? Content
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
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
    protected override void SetBindingItemTemplate()
    {
        //base.SetBindingItemTemplate();
    }
    protected virtual void SetBindingContentTemplate()
    {
        ContentTemplate = RegionContentTemplate;
        _tabControl.Bind(TabControl.ContentTemplateProperty,
                               new Binding(nameof(ContentTemplate))
                               {
                                   Mode = BindingMode.TwoWay,
                                   Source = this
                               });
    }
}
