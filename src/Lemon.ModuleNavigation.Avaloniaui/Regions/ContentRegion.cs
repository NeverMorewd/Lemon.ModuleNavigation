using Avalonia.Controls;
using Lemon.ModuleNavigation.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions
{
    public class ContentRegion : AvaloniauiRegion
    {
        private readonly ContentControl _contentControl;
        public ContentRegion(ContentControl contentControl) : base()
        {
            _contentControl = contentControl;
            Contexts = [];
            Contexts.CollectionChanged += ViewContents_CollectionChanged;
            _contentControl.ContentTemplate = ContainerTemplate;
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
                if (target.ViewName == current.ViewName)
                {
                    return;
                }
            }
            Content = target;
        }

        public override void DeActivate(NavigationContext target)
        {
            Content = null;
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
}
