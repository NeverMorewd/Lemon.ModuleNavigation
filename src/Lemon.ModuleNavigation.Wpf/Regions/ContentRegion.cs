using Lemon.ModuleNavigation.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace Lemon.ModuleNavigation.Wpf.Regions
{
    public class ContentRegion : Region
    {
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
