namespace Lemon.ModuleNavigation.Sample.ModuleCs.Services
{
    public class SomeService : ISomeService
    {
        public int GetCode()
        {
            return 0;
        }
    }
    public interface ISomeService
    {
        int GetCode();
    }
}
