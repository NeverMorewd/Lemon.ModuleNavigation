namespace Lemon.ModuleNavigation.Extensions
{
    public static class ObservableExtension
    {
        public static IDisposable NavigationSubscribe<T>(this IObservable<T> observable, Action<T> onNext)
        {
            var observer = new NavigationObserver<T>(onNext);
            return observable.Subscribe(observer);
        }
        private class NavigationObserver<T> : IObserver<T>
        {
            private readonly Action<T> _onNext;

            public NavigationObserver(Action<T> onNext)
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
