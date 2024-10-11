using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui;

public abstract class AvaModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TView,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes
        .PublicConstructors)]
    TViewModel> : Module<TView, TViewModel> where TViewModel : IViewModel where TView : IView
{
    private IView? _viewForDataTemplate;
    public AvaModule(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }

    /// <summary>
    /// For:the view of one module needs to be displayed in multiple parents
    /// </summary>
    public IDataTemplate ViewTemplate
        => new FuncDataTemplate<IModule>((m, np) =>
        {
            if (_viewForDataTemplate == null)
            {
                _viewForDataTemplate = _serviceProvider.GetRequiredKeyedService<IView>(Key);
                var viewModel = _serviceProvider.GetRequiredKeyedService<IViewModel>(Key);
                _viewForDataTemplate.SetDataContext(viewModel);
            }
            return _viewForDataTemplate as Control;
        });
}