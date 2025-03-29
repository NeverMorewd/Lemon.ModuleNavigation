using Lemon.ModuleNavigation.Abstractions;
using Lemon.ModuleNavigation.Wpf;
using System.Windows.Controls;
using System.Windows.Threading;

namespace System.Windows
{
    public static class WpfExtensions
    {
        public static T WaitOnDispatcherFrame<T>(this Task<T> task)
        {
            if (!task.IsCompleted)
            {
                var frame = new DispatcherFrame();
                task.ContinueWith(static (_, s) => ((DispatcherFrame)s!).Continue = false, frame);
                Dispatcher.PushFrame(frame);
            }

            return task.GetAwaiter().GetResult();
        }

        public static T? FindLogicalAncestorOfType<T>(this DependencyObject obj, bool includeSelf = false) where T : DependencyObject
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (includeSelf && obj is T self)
                return self;

            DependencyObject? parent = LogicalTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T ancestor)
                    return ancestor;

                parent = LogicalTreeHelper.GetParent(parent);
            }

            return null;
        }

        public static IRegion ToContainer(this Control control, string name)
        {
            return control switch
            {
                TabControl tabControl => new TabRegion(tabControl, name),
                ItemsControl itemsControl => new ItemsRegion(itemsControl, name),
                ContentControl contentControl => new ContentRegion(contentControl, name),
                _ => throw new NotSupportedException($"Unsupported control:{control.GetType()}"),
            };
        }
    }
}
