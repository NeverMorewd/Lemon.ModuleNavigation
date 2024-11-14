using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Avaloniaui.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection serviceDescriptors, string viewKey)
            where TView : class, IView
            where TViewModel : class, INavigationAware
        {

            serviceDescriptors
                .AddTransient<TViewModel>()
                .AddTransient<TView>()
                .AddKeyedTransient<IView>(viewKey, (sp, key) =>
                {
                    var view = sp.GetRequiredService<TView>();
                    return view;
                })
                .AddKeyedTransient<INavigationAware>(viewKey, (sp, key) =>
                {
                    var viewModel = sp.GetRequiredService<TViewModel>();
                    return viewModel;
                });
            if (typeof(TViewModel).IsAssignableTo(typeof(IDialogAware)))
            {
                serviceDescriptors.AddKeyedTransient(viewKey, (sp, key) =>
                {
                    var viewModel = sp.GetRequiredService<TViewModel>();
                    return (IDialogAware)viewModel;
                });
            }
            return serviceDescriptors;
        }

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
