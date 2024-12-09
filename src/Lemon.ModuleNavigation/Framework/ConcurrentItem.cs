using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Framework
{
    public class ConcurrentItem<T>
    {
        private const int SIZE = 1;
        private readonly BlockingCollection<T> _collection = new(SIZE);
        public ConcurrentItem() { }

        public void SetData(T data)
        {
            if (_collection.Count == SIZE)
            {
                _collection.Take();
            }
            _collection.Add(data);
        }

        public T TakeData()
        {
            return _collection.Take();
        }

        public bool TryTakeData(out T? data)
        {
            return _collection.TryTake(out data);
        }

        public bool WaitForData(TimeSpan timeSpan, out T? data)
        {
            return _collection.TryTake(out data, timeSpan);
        }
    }

}
