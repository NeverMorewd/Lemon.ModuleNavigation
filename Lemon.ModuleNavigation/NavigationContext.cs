using Lemon.ModuleNavigation.Abstracts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lemon.ModuleNavigation
{
    public class NavigationContext : INavigationHandler<IModule>, IDisposable, INotifyPropertyChanged
    {
        private readonly INavigationService<IModule> _navigationService;
        private readonly IDisposable _navigationCleanup;
        public NavigationContext(INavigationService<IModule> navigationService,
            IEnumerable<IModule> modules) 
        {
            _navigationService = navigationService;
            Modules = modules;
            ActiveModules = new ObservableCollection<IModule>(Modules
                    .Where(m =>
                    {
                        Console.WriteLine($"Find a module:{m.Key}");
                        return !m.LoadOnDemand;
                    })
                    .Select(m =>
                    {
                        Console.WriteLine($"Initialize module:{m.Key}");
                        m.Initialize();
                        return m;
                    }));
            _navigationCleanup = _navigationService.OnNavigation(this);
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

        public void Dispose()
        {
            _navigationCleanup.Dispose();
        }

        public void NavigateTo(IModule target)
        {
            if (target.AllowMultiple)
            {
                ActiveModules.Add(target);
            }
            else
            {
                if (!ActiveModules.Contains(target))
                {
                    ActiveModules.Add(target);
                }
            }

            ///TODO:Consider an async implementation
            target.Initialize();
            target.IsActivated = true;
            CurrentModule = target;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
