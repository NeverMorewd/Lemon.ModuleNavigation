namespace Lemon.ModuleNavigation.Abstractions;

public interface ICanUnload
{
    event Action? RequestUnload;
}
