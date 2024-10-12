using Avalonia;
using Avalonia.Markup.Xaml;

namespace Lemon.ModuleNavigation.Sample;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

}
