using Lemon.Toolkit.Models;
using System;
using System.Reactive.Subjects;

namespace Lemon.Toolkit.Services
{
    public class ShellService : IObservable<ShellParamModel>, IObserver<ShellParamModel>
    {
        private readonly Subject<ShellParamModel> _subject = new();
        public ShellService()
        {

        }

        public IDisposable Subscribe(IObserver<ShellParamModel> observer)
        {
            return _subject.Subscribe(observer);
        }

        public void OnCompleted()
        {
            _subject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            _subject.OnError(error);
        }

        public void OnNext(ShellParamModel value)
        {
            _subject.OnNext(value);
            Console.WriteLine($"Send to Shell:{value}");
        }
    }
}
