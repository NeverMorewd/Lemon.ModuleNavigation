using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Lemon.Toolkit.Comparer.Services
{
    public class ConsoleService : TextWriter
    {
        private readonly TextWriter _originalOutput = Console.Out;
        private readonly StringWriter _stringWriter = new();
        private readonly Subject<string?> _outputSubject = new();

        public IObservable<string?> OutputObservable => _outputSubject.AsObservable();
        public override Encoding Encoding => _originalOutput.Encoding;
        public string GetOutput()
        {
            return _stringWriter.ToString();
        }

        public override void Write(char value)
        {
            _originalOutput.Write(value);
            _outputSubject.OnNext(value.ToString());
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
                _stringWriter.Dispose();
                _outputSubject.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
