using Lemon.ModuleNavigation.Abstracts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lemon.ModuleNavigation.Avaloniaui.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        public HomeViewModel(IEnumerable<IModule> modules,
            NavigationService navigationService)
        {
            _navigationService = navigationService;
            Modules = new ObservableCollection<IModule>(modules);
        }
        public ObservableCollection<IModule> Modules
        {
            get;
            set;
        }
        private IModule? _selectedModule;

        public IModule? SelectedModule
        {
            get => _selectedModule;
            set
            {
                if (_selectedModule != value)
                {
                    _selectedModule = value;
                    OnPropertyChanged();
                    _navigationService.NavigateTo(_selectedModule!);
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
