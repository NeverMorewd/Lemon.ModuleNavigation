namespace Lemon.ModuleNavigation.Core;

public class RegionNameNotFoundException : Exception
{
    public RegionNameNotFoundException(string regionName) : base($"{regionName} was not found!")
    {
        
    }
}
