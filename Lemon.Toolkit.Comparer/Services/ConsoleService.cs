using Lemon.Toolkit.Domains;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Lemon.Toolkit.Services
{
    public class ConsoleService : StringWriter, IConsoleService
    {
        private readonly TextWriter _originalOutput = Console.Out;
        private readonly TextWriter _originalError = Console.Error;
        private readonly ReplaySubject<string?> _outputSubject = new();
        private readonly ReplaySubject<string?> _errorSubject = new();

        public IObservable<string?> OutputObservable => _outputSubject.AsObservable();
        public IObservable<string?> ErrorObservable => _errorSubject.AsObservable();
        public override Encoding Encoding => _originalOutput.Encoding;

        public void Error(string error)
        {
            _errorSubject.OnNext(error);
        }
        public void Output(string content)
        {
            _outputSubject.OnNext(content);
        }
        public override void Write(string? value)
        {
            _originalOutput.Write(value);
            _outputSubject.OnNext(value);
        }

        public override void WriteLine(string? value)
        {
            _originalOutput.WriteLine(value);
            _outputSubject.OnNext(value);
        }

        public override void WriteLine(string format, params object?[] arg)
        {
            _originalOutput.WriteLine(format, arg);
            _outputSubject.OnNext(string.Format(format, arg));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Console.SetOut(_originalOutput);
                Console.SetError(_originalError);
                _outputSubject.Dispose();
                _errorSubject.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
