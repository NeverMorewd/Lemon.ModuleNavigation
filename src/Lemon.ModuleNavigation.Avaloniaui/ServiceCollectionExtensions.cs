using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Avaloniaui.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAvaNavigationSupport(this IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors
                    .AddNavigationSupport()
                    .AddSingleton(sp => sp.GetKeyedServices<IView>(typeof(IView)))
                    .AddSingleton<IDialogService, AvaDialogService>()
                    .AddKeyedTransient<IAvaDialogWindow, DefaultDialogWindow>(DefaultDialogWindow.Key);
        }

        public static IServiceCollection AddAvaDialogWindow<TDialogWindow>(this IServiceCollection serviceDescriptors, string windowKey) 
            where TDialogWindow : class, IAvaDialogWindow
        {
            return serviceDescriptors.AddKeyedTransient<IAvaDialogWindow, TDialogWindow>(windowKey);
        }
        //public static IServiceCollection AddAvaDialog<TView, TViewModel>(this IServiceCollection serviceDescriptors, string viewKey) 
        //    where TViewModel : class, IDialogAware
        //    where TView : class, IView
        //{
        //    return serviceDescriptors
        //        .AddKeyedTransient<IDialogAware, TViewModel>(viewKey)
        //        .AddKeyedTransient<IView, TView>(viewKey);
        //}
    }
}
