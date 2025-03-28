using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Wpf.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Wpf.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWpfNavigationSupport(this IServiceCollection serviceDescriptors)
    {
        return serviceDescriptors
                .AddNavigationSupport()
                .AddSingleton(sp => sp.GetKeyedServices<IView>(typeof(IView)))
                .AddSingleton<IDialogService, DialogService>()
                .AddKeyedTransient<IDialogWindow, DefaultDialogWindow>(DefaultDialogWindow.Key);
    }

    public static IServiceCollection AddDialogWindow<TDialogWindow>(this IServiceCollection serviceDescriptors, string windowKey)
        where TDialogWindow : class, IDialogWindow
    {
        return serviceDescriptors.AddKeyedTransient<IDialogWindow, TDialogWindow>(windowKey);
    }
}
