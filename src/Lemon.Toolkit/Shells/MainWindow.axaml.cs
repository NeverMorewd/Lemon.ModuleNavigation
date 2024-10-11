using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.Logging;

namespace Lemon.Toolkit.Shells
{
    public partial class MainWindow : Window
    {
        private readonly ILogger _logger;
        public MainWindow(ILogger<MainWindow> logger)
        {
            InitializeComponent();
            _logger = logger;
        }
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
        }
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            var hWnd = TryGetPlatformHandle()!.Handle;
            _logger.LogInformation($"MainWindow handle:{hWnd}");
        }
    }
}