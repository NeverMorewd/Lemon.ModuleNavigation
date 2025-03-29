using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Wpf;

namespace Microsoft.Extensions.DependencyInjection;

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
