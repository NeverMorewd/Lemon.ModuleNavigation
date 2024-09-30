using Lemon.Toolkit.Domains;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Lemon.Toolkit.Log
{
    public class UILoggerProvider : ILoggerProvider
    {
        private readonly string _name;
        private readonly IConsoleService _consoleService;
        private readonly LogLevel _minimumLevel;

        public UILoggerProvider(string name, 
            IConsoleService consoleService,
            LogLevel minimumLevel)
        {
            _name = name;
            _consoleService = consoleService;
            _minimumLevel = minimumLevel;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new UILogger(_name, 
                categoryName, 
                _consoleService, 
                _minimumLevel);
        }

        public void Dispose()
        {
            _consoleService?.Dispose();
        }
    }
}
