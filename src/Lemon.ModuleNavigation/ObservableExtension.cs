namespace Lemon.ModuleNavigation
{
    public static class ObservableExtension
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext)
        {
            var observer = new DelegateObserver<T>(onNext);
            return observable.Subscribe(observer);
        }

        private class DelegateObserver<T> : IObserver<T>
        {
            private readonly Action<T> _onNext;

            public DelegateObserver(Action<T> onNext)
            {
                _onNext = onNext ?? throw new ArgumentNullException(nameof(onNext));
            }

            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(T value)
            {
                _onNext(value);
            }
        }
    }
}
