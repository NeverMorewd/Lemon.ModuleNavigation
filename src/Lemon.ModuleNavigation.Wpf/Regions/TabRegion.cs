using Lemon.ModuleNavigation.Abstractions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Lemon.ModuleNavigation.Wpf.Regions;

public class TabRegion : ItemsRegion, IContentRegionContext<DataTemplate>
{
    private readonly TabControl _tabControl;
    public TabRegion(string name, TabControl tabControl) : base(name, tabControl)
    {
        _tabControl = tabControl;
        ContentTemplate = RegionTemplate;
        SetBindingContentTemplate();
    }
    public object? Content
    {
        get;
        set;
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
    protected override void SetBindingItemTemplate()
    {
        //
    }

    protected virtual void SetBindingContentTemplate()
    {
        BindingOperations.SetBinding(_tabControl,
            TabControl.ContentTemplateProperty,
            new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(ContentTemplate)),
            });
    }
}
