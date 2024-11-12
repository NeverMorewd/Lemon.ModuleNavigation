using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Lemon.ModuleNavigation.Avaloniaui.Regions
{
    public abstract class AvaloniauiRegion : IRegion
    {
        private readonly Dictionary<string, IView> _viewCache;
        private readonly ConcurrentItem<(IView, INavigationAware)> _current;
        public AvaloniauiRegion()
        {
            _viewCache = [];
            _current = new();
            RegionTemplate = GetDataTemplate();
        }
        public abstract string Name
        {
            get;
        }
        public abstract ObservableCollection<NavigationContext> Contexts
        {
            get;
        }
        public IDataTemplate? RegionTemplate
        {
            get;
            set;
        }

        public abstract void Activate(NavigationContext target);

        public abstract void DeActivate(NavigationContext target);

        private IDataTemplate GetDataTemplate()
        {
            return new FuncDataTemplate<NavigationContext>((context, np) =>
            {
                if (context == null)
                {
                    return default;
                }
                if (context.RequestNew || !_viewCache.TryGetValue(context.TargetViewName, out IView? view))
                {
                    view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.TargetViewName);

                    var viewFullName = view.GetType().FullName;

                    context.Uri = new Uri($"avares://{viewFullName}.axaml");
                    var navigationAware = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.TargetViewName);
                    if (_current.TryTakeData(out (IView, INavigationAware) data))
                    {
                        data.Item2.OnNavigatedFrom(context);
                    }
                    if (navigationAware.IsNavigationTarget(context))
                    {
                        view.DataContext = navigationAware;
                        navigationAware.OnNavigatedTo(context);
                        _current.SetData((view, navigationAware));
                    }
                    else
                    {
                        return default;
                    }

                    _viewCache.TryAdd(context.TargetViewName, view);
                }
                return view as Control;
            });
        }
    }
}
