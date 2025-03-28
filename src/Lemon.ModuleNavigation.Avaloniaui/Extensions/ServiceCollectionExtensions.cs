using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Avaloniaui.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAvaNavigationSupport(this IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors
                    .AddNavigationSupport()
                    .AddSingleton(sp => sp.GetKeyedServices<IView>(typeof(IView)))
                    .AddSingleton<IDialogService, DialogService>()
                    .AddKeyedTransient<IDialogWindow, DefaultDialogWindow>(DefaultDialogWindow.Key);
        }

        public static IServiceCollection AddAvaDialogWindow<TDialogWindow>(this IServiceCollection serviceDescriptors, string windowKey)
            where TDialogWindow : class, IDialogWindow
        {
            return serviceDescriptors.AddKeyedTransient<IDialogWindow, TDialogWindow>(windowKey);
        }
    }
}
