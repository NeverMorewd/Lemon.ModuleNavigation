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
        public AvaloniauiRegion()
        {
            _viewCache = [];
            RegionTemplate = GetDataTemplate();
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
                if (context.RequestNew || !_viewCache.TryGetValue(context.ViewName, out IView? view))
                {
                    view = context.ServiceProvider.GetRequiredKeyedService<IView>(context.ViewName);

                    var viewFullName = view.GetType().FullName;

                    context.Uri = new Uri($"avares://{viewFullName}.axaml");
                    var viewModel = context.ServiceProvider.GetRequiredKeyedService<INavigationAware>(context.ViewName);
                    viewModel.OnNavigatedTo(context);
                    if (viewModel.IsNavigationTarget(context))
                    {
                        view.DataContext = viewModel;
                    }
                    else
                    {
                        return default;
                    }
                    _viewCache.TryAdd(context.ViewName, view);
                }
                return view as Control;
            });
        }
    }
}
