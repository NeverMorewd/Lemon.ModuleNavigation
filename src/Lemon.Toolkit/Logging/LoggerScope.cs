using System;

namespace Lemon.Toolkit.Logging
{
    public class LoggerScope : IDisposable
    {
        private readonly LoggerScope _parent;
        private readonly string _state;

        public LoggerScope(LoggerScope parent, string state)
        {
            _parent = parent;
            _state = state;
            Current = this;
        }

        public static LoggerScope? Current { get; private set; }

        public void Dispose()
        {
            Current = _parent;
        }

        public override string ToString()
        {
            return _state;
        }
    }

}
