using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lemon.ModuleNavigation
{
    public class NavigationHandler : INavigationHandler, IDisposable, INotifyPropertyChanged
    {
        private readonly INavigationService<IModule> _navigationService;
        private readonly IDisposable _navigationCleanup;
        private readonly IDisposable _viewNavigationCleanup;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, IModule> _modulesCache;
        public NavigationHandler(INavigationService<IModule> navigationService,
            IViewNavigationService viewNavigationService,
            IEnumerable<IModule> modules,
            IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _modulesCache = new ConcurrentDictionary<string, IModule>(modules.ToDictionary(m=>m.Key,m=>m));
            Modules = _modulesCache.Select(m=>m.Value);
            ActiveModules = new ObservableCollection<IModule>(_modulesCache
                    .Where(m =>
                    {
                        Console.WriteLine($"Find a module:{m.Key}");
                        return !m.Value.LoadOnDemand;
                    })
                    .Select(m =>
                    {
                        Console.WriteLine($"Initialize module:{m.Key}");
                        m.Value.Initialize();
                        return m.Value;
                    }));
            _navigationCleanup = _navigationService.BindingNavigationHandler(this);
            _viewNavigationCleanup = viewNavigationService.BindingViewNavigationHandler(this);
        }

        public ObservableCollection<IModule> ActiveModules
        {
            get;
            set;
        }
        public IEnumerable<IModule> Modules
        {
            get;
            set;
        }
        public IServiceProvider ServiceProvider => _serviceProvider;

        private IModule? _currentModule;
        public IModule? CurrentModule
        {
            get => _currentModule;
            set
            {
                if (_currentModule != value)
                {
                    _currentModule = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnNavigateTo(IModule module)
        {
            OnNavigateToCore(module);
        }
        public void OnNavigateTo(string moduleKey)
        {
            if(_modulesCache.TryGetValue(moduleKey, out var module))
            {
                OnNavigateToCore(module);
            }
            else
            {
                throw new InvalidOperationException($"Invalid module key:{moduleKey}");
            }
        }
        public IView CreateNewView(IModule module)
        {
            if (module != null)
            {
                var view = _serviceProvider.GetRequiredKeyedService<IView>(module.Key);
                var viewmodel = _serviceProvider.GetRequiredKeyedService<IViewModel>(module.Key);
                view.DataContext = viewmodel;
                return view;
            }
            throw new ArgumentNullException(nameof(module));
        }
        public IViewModel CreateNewViewModel(IModule module)
        {
            return _serviceProvider.GetRequiredKeyedService<IViewModel>(module.Key);
        }

        public virtual void OnNavigateTo(string containerName, 
            string viewName, 
            bool requestNew = false)
        {
            
        }

        private void OnNavigateToCore(IModule module)
        {
            if (module.ForceNew)
            {
                module = _serviceProvider.GetKeyedService<IModule>(module.Key)!;
                ActiveModules.Add(module);
            }
            else
            {
                if (!ActiveModules.Contains(module))
                {
                    ActiveModules.Add(module);
                }
            }

            ///TODO:Consider an async implementation
            module.Initialize();
            module.IsActivated = true;
            CurrentModule = module;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Dispose()
        {
            _navigationCleanup?.Dispose();
            _viewNavigationCleanup?.Dispose();
        }
    }
}
