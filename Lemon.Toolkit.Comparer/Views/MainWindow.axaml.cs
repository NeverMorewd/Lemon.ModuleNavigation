using Avalonia;
using Avalonia.Controls;
using System.Runtime.InteropServices;
using System;
using System.Reactive.Linq;
using ReactiveUI;
using Avalonia.Interactivity;

namespace Lemon.Toolkit.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
        }
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            var hWnd = TryGetPlatformHandle().Handle;
            Observable.Interval(TimeSpan.FromSeconds(1))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    NativeMethods.UpdateWindow(hWnd);
                });
        }
    }

    public class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UpdateWindow(IntPtr hWnd);
    }
}