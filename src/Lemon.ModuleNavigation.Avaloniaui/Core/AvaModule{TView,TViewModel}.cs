using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using System.Diagnostics.CodeAnalysis;

namespace Lemon.ModuleNavigation.Avaloniaui.Core;

[Obsolete($"Use Module instead, they are equivalent.")]
public abstract class AvaModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>
    : Module<TView, TViewModel>
    where TViewModel : IModuleNavigationAware
    where TView : IView
{
    public AvaModule(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }
}