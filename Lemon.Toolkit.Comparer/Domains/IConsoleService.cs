using System;

namespace Lemon.Toolkit.Domains
{
    public interface IConsoleService : IDisposable
    {
        public void Error(string error);
        public void Output(string content);
    }
}
