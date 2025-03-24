using Avalonia.Threading;

namespace Lemon.ModuleNavigation.Avaloniaui.Extensions
{
    public static class AvaloniauiExtensions
    {
        public static T WaitOnDispatcherFrame<T>(this Task<T> task)
        {
            if (!task.IsCompleted)
            {
                var frame = new DispatcherFrame();
                task.ContinueWith(static (_, s) => ((DispatcherFrame)s!).Continue = false, frame);
                Dispatcher.UIThread.PushFrame(frame);
            }

            return task.GetAwaiter().GetResult();
        }
    }
}
