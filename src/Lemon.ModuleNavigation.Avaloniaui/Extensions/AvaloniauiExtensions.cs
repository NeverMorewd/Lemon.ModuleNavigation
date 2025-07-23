using Avalonia.Threading;

namespace Avalonia;

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

    public static async Task UIInvokeAsync(Action action)
    {
        await Dispatcher.UIThread.InvokeAsync(action);
        //if (!Dispatcher.UIThread.CheckAccess())
        //    await Dispatcher.UIThread.InvokeAsync(action);
        //else
        //    action();
    }

    public static T UIInvoke<T>(Func<T> action)
    {
        if (!Dispatcher.UIThread.CheckAccess())
            return Dispatcher.UIThread.Invoke(action);
        else
            return action();
    }
}
