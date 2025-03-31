using Avalonia.Controls;
using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Lemon.ModuleNavigation.Avaloniaui;

public class ContentRegion : Region
{
    private readonly ConcurrentDictionary<string, IView> _viewCache = new();
    private readonly ConcurrentItem<(IView View, INavigationAware NavigationAware)> _current = new();
    private readonly ContentControl _contentControl;
    public ContentRegion(ContentControl contentControl, string name) : base()
    {
        _contentControl = contentControl;
        _contentControl.ContentTemplate = RegionTemplate;
        Contexts = [];
        Contexts.CollectionChanged += ViewContents_CollectionChanged;
        Name = name;
    }

    public override string Name
    {
        get;
    }
    public object? Content 
    { 
        get => _contentControl.Content;
        set
        {
            _contentControl.Content = value;
        }
    }
    public override ObservableCollection<NavigationContext> Contexts
    {
        get;
    }

    public override void Activate(NavigationContext target)
    {
        if(Content is NavigationContext current)
        {
            if (target.TargetViewName == current.TargetViewName 
                && !target.RequestNew)
            {
                return;
            }
        }
        Content = target;
        Contexts.Add(target);
    }

    public override void DeActivate(string regionName)
    {
        if (Content is NavigationContext current)
        {
            if (current.TargetViewName == regionName)
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
            if (current == navigationContext)
            {
                Contexts.Remove(current);
                Content = null;
            }
        }
    }
    private void ViewContents_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            //
        }
        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            //
        }
    }
}
