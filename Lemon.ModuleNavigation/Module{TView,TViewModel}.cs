using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.ModuleNavigation
{
    public abstract class Module<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel> : IModule where TViewModel : IViewModel where TView : IView
    {
        public string Key => GetType().Name;
        public readonly IServiceProvider _serviceProvider;
        public Module(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public virtual void Initialize()
        {
            lock (this)
            {
                if (IsInitialized) return;
                View = _serviceProvider.GetRequiredKeyedService<IView>(Key);
                ViewModel = _serviceProvider.GetRequiredKeyedService<IViewModel>(Key);
                View.SetDataContext(ViewModel);
                IsInitialized = true;
            }
        }
        public IView? View
        {
            get;
            protected set;
        }
        public IViewModel? ViewModel
        {
            get;
            protected set;
        }
        public Type ViewType
        {
            get
            {
                return typeof(TView);
            }
        }

        public Type ViewModelType
        {
            get
            {
                return typeof(TViewModel);
            }
        }
        public abstract bool LoadOnDemand
        {
            get;
        }
        public bool IsInitialized
        {
            get;
            protected set;
        } = false;
        public bool IsActivated
        {
            get;
            set;
        } = false;
        public virtual bool AllowMultiple
        {
            get => false;
        } 
        public virtual string? Alias => Key;
    }
}
