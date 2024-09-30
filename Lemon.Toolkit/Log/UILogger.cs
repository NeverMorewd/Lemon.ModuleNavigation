using Lemon.Toolkit.Domains;
using Microsoft.Extensions.Logging;
using System;

namespace Lemon.Toolkit.Log
{
    public class UILogger : ILogger
    {
        private readonly string _providerName;
        private readonly string _categoryName;
        private readonly IConsoleService _consoleService;
        private readonly int _processId;
        private IDisposable? _currentScope;
        private readonly LogLevel _minimumLevel;

        public UILogger(string providerName, 
            string categoryName, 
            IConsoleService consoleService,
            LogLevel minimumLevel)
        {
            _minimumLevel = minimumLevel;
            _providerName = providerName;
            _categoryName = categoryName;
            _consoleService = consoleService;
            _processId = Environment.ProcessId;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState: notnull
        {
            _currentScope = new LoggerScope(LoggerScope.Current!, state!.ToString()!);
            return _currentScope;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minimumLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            var logAction = _consoleService.Output;
            if (logLevel == LogLevel.Error)
            {
                logAction = _consoleService.Error;
            }
            string message = formatter(state, exception);
            if (_currentScope != null)
            {
                message = $"#{_currentScope}# {message}";
            }
            logAction($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}-[{_processId}]-[{Environment.CurrentManagedThreadId}]-[{_categoryName}]-[{logLevel}]: {message}");
            if (exception != null)
            {
                logAction(exception.ToString());
            }
        }
    }
}
