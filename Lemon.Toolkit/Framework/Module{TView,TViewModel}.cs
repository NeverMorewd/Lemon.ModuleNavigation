using Lemon.Toolkit.Framework.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.Toolkit.Framework
{
    public abstract class Module<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel> : IModule where TViewModel : IViewModel where TView : IView
    {
        public string Name => GetType().Name;
        public readonly IServiceProvider _serviceProvider;
        public Module(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public virtual void Initialize()
        {
            lock (this)
            {
                if (!IsInitialized)
                {
                    View = _serviceProvider.GetRequiredKeyedService<IView>(Name);
                    ViewModel = _serviceProvider.GetRequiredKeyedService<IViewModel>(Name);
                    View.SetDataContext(ViewModel);
                    IsInitialized = true;
                }
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
    }
}
