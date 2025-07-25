using Lemon.ModuleNavigation.Abstractions;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive;

namespace Lemon.ModuleNavigation.SampleViewModel
{
    public class AsyncBaseNavigationViewModel : ReactiveObject, IAsyncNavigationAware, IAsyncCanUnload
    {
        public virtual string Greeting => $"Welcome to {GetType().Name}[{Environment.ProcessId}][{Environment.CurrentManagedThreadId}]{Environment.NewLine}{DateTime.Now:yyyy-MM-dd HH-mm-ss.ffff}";
        public virtual string? Alias => GetType().Name;
        public AsyncBaseNavigationViewModel()
        {
            UnloadViewCommand = ReactiveCommand.CreateFromTask(async() =>
            {
                var code = this.GetHashCode();
                Debug.WriteLine(code);
                if(RequestUnloadAsync is not null)
                    await RequestUnloadAsync.Invoke();
            });
        }
        public ReactiveCommand<Unit, Unit> UnloadViewCommand
        {
            get;
        }

        public event Func<Task>? RequestUnloadAsync;

        public virtual async  Task OnNavigatedToAsync(NavigationContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
        }

        public virtual Task OnNavigatedFromAsync(NavigationContext context)
        {
            return Task.CompletedTask;
        }

        public virtual Task<bool> IsNavigationTargetAsync(NavigationContext context)
        {
            return Task.FromResult(true);
        }
    }
}
