using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ModuleNavigation.Avaloniaui;

public abstract class AvaModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes
        .PublicConstructors)]
    TViewModel> : Module<TView, TViewModel> where TViewModel : IViewModel where TView : IView
{
    public AvaModule(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public IDataTemplate ViewTemplate 
        => new FuncDataTemplate<TViewModel>((_, __) =>
        {
            return View as Control;
        });
}