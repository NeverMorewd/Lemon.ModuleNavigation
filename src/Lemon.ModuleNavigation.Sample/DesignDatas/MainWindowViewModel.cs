using Avalonia;
using Lemon.ModuleNavigation.Sample.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ModuleNavigation.Sample.DesignDatas;

public static class DesignData
{
    public static MainViewModel MainWindowViewModel { get; } =
        ((AppWithDi)Application.Current!).AppServiceProvider!.GetRequiredService<MainViewModel>();
}
