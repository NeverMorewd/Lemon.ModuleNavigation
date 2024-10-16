using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.ModuleNavigation
{
    public abstract class ScopeModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>
        : Module<TView, TViewModel>, IModuleScope 
        where TViewModel : IViewModel 
        where TView : IView
    {
        public ScopeModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        public void MapSerivce<T>() where T : class
        {
            var service = _serviceProvider.GetService<T>();
            if (service != null)
            {
                ScopeServiceCollection.AddSingleton<T>(service);
            }
        }
        public IServiceCollection ScopeServiceCollection
        {
            get;
        }
        public IServiceProvider ScopeServiceProvider
        {
            get;
        }
    }
}
