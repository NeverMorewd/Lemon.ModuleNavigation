namespace Lemon.ModuleNavigation.Abstractions;

public interface IAsyncCanUnload
{
    event Func<Task>? RequestUnloadAsync;
}
