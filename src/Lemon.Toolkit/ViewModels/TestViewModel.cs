using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.Toolkit.ViewModels
{
    public class TestViewModel : ViewModelBase, IViewModel
    {
        private static readonly TextBlock _text = new() { Text="I am static!" };
        public IDataTemplate TestNewTemplate
        => new FuncDataTemplate<string>((x, __) =>
        {
            return new TextBlock { Text = $"test:{x}" };
        });

        public IDataTemplate TestTemplate
        => new FuncDataTemplate<string>((_, __) =>
        {
            return _text;
        });
    }
}
