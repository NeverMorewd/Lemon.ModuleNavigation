using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Avaloniaui
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterView<TView, TViewModel>(this IServiceCollection serviceDescriptors)
            where TView : UserControl
            where TViewModel : class
        {
            return
            serviceDescriptors
                .AddTransient<TViewModel>()
                .AddKeyedTransient<UserControl>(typeof(TView).Name, (sp, key) =>
                {
                    var viewModel = sp.GetRequiredService<TViewModel>();
                    var view = ActivatorUtilities.CreateInstance<TView>(sp);
                    view.DataContext = viewModel;
                    return view;
                });
        }

        public static IServiceCollection AddAvaNavigationSupport(this IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors
                    .AddNavigationSupport()
                    .AddSingleton(sp => sp.GetKeyedServices<UserControl>(typeof(UserControl)))
                    .AddSingleton<AvaNavigationContext>();

        }
    }
}
