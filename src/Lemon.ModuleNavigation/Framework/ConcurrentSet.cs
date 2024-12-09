using System.Collections;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Internals
{
    internal sealed class ConcurrentSet<T> : IEnumerable<T> where T : notnull
    {
        private readonly ConcurrentDictionary<T, byte> _dict = new();

        public bool Add(T item)
        {
            return _dict.TryAdd(item, 0);
        }

        public bool Remove(T item)
        {
            return _dict.TryRemove(item, out _);
        }

        public bool Contains(T item)
        {
            return _dict.ContainsKey(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dict.Select(kv => kv.Key).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public int Count => _dict.Count;
    }
}
