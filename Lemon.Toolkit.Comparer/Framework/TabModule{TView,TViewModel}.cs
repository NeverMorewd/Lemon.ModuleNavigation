using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.Toolkit.Framework
{
    public abstract class TabModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel> : ITabModule where TViewModel : IViewModel where TView : IView
    {
        public string Name => GetType().Name;
        public readonly IServiceProvider _serviceProvider;
        public TabModule(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public virtual void Initialize()
        {
            View = _serviceProvider.GetRequiredKeyedService<IView>(Name);
            ViewModel = _serviceProvider.GetRequiredKeyedService<IViewModel>(Name);
            View.SetDataContext(ViewModel);
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
        public abstract bool LoadDefault
        {
            get;
        }
    }
}
