using System.Collections;
using System.Collections.Concurrent;

namespace Lemon.ModuleNavigation.Internal;

/// <summary>
/// Thread-safe set implementation based on ConcurrentDictionary
/// </summary>
/// <typeparam name="T">Element type</typeparam>
internal sealed class ConcurrentSet<T> : IEnumerable<T>, IReadOnlyCollection<T> where T : notnull
{
    private readonly ConcurrentDictionary<T, byte> _dict = new();

    /// <summary>
    /// Adds an element to the set
    /// </summary>
    /// <param name="item">The element to add</param>
    /// <returns>Returns true if the element was successfully added, false if the element already exists</returns>
    public bool Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return _dict.TryAdd(item, 0);
    }

    /// <summary>
    /// Removes an element from the set
    /// </summary>
    /// <param name="item">The element to remove</param>
    /// <returns>Returns true if the element was successfully removed, false if the element does not exist</returns>
    public bool Remove(T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return _dict.TryRemove(item, out _);
    }

    /// <summary>
    /// Checks if the set contains the specified element
    /// </summary>
    /// <param name="item">The element to check</param>
    /// <returns>Returns true if the element is contained, otherwise false</returns>
    public bool Contains(T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return _dict.ContainsKey(item);
    }

    /// <summary>
    /// Attempts to add an element; does nothing if the element already exists
    /// </summary>
    /// <param name="item">The element to add</param>
    /// <returns>Returns true if the element was successfully added or already exists</returns>
    public bool TryAdd(T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        _dict.TryAdd(item, 0);
        return true; // Always returns true regardless of whether it already exists
    }

    /// <summary>
    /// Adds or updates an element (for a Set, this ensures the element exists)
    /// </summary>
    /// <param name="item">The element to add</param>
    public void AddOrUpdate(T item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        _dict.AddOrUpdate(item, 0, (key, oldValue) => 0);
    }

    /// <summary>
    /// Attempts to remove an element
    /// </summary>
    /// <param name="item">The element to remove</param>
    /// <param name="removed">Output parameter indicating whether the removal was successful</param>
    /// <returns>Returns true if the element existed and was successfully removed</returns>
    public bool TryRemove(T item, out bool removed)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        removed = _dict.TryRemove(item, out _);
        return removed;
    }

    /// <summary>
    /// Clears all elements from the set
    /// </summary>
    public void Clear()
    {
        _dict.Clear();
    }

    /// <summary>
    /// Copies the elements of the set to an array
    /// </summary>
    /// <returns>An array containing all elements of the set</returns>
    public T[] ToArray()
    {
        return [.. _dict.Keys];
    }

    /// <summary>
    /// Copies the elements of the set to the specified array
    /// </summary>
    /// <param name="array">The destination array</param>
    /// <param name="arrayIndex">The starting index for copying</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array, nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Array index cannot be negative");

        var keys = _dict.Keys;
        if (arrayIndex + keys.Count > array.Length)
            throw new ArgumentException("Destination array is not large enough");

        keys.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Adds multiple elements to the set
    /// </summary>
    /// <param name="items">The collection of elements to add</param>
    /// <returns>The number of elements successfully added</returns>
    public int AddRange(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        int addedCount = 0;
        foreach (var item in items)
        {
            if (item != null && Add(item))
            {
                addedCount++;
            }
        }
        return addedCount;
    }

    /// <summary>
    /// Removes multiple elements from the set
    /// </summary>
    /// <param name="items">The collection of elements to remove</param>
    /// <returns>The number of elements successfully removed</returns>
    public int RemoveRange(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        int removedCount = 0;
        foreach (var item in items)
        {
            if (item != null && Remove(item))
            {
                removedCount++;
            }
        }
        return removedCount;
    }

    /// <summary>
    /// Gets a value indicating whether the set is empty
    /// </summary>
    public bool IsEmpty => _dict.IsEmpty;

    /// <summary>
    /// Gets the number of elements in the set
    /// </summary>
    public int Count => _dict.Count;

    /// <summary>
    /// Gets an enumerator for the set
    /// </summary>
    /// <returns>An enumerator for the set</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return _dict.Keys.GetEnumerator();
    }

    /// <summary>
    /// Gets a non-generic enumerator for the set
    /// </summary>
    /// <returns>A non-generic enumerator for the set</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Gets a snapshot of the set (all elements at the current moment)
    /// </summary>
    /// <returns>A read-only collection containing all current elements</returns>
    public IReadOnlyCollection<T> GetSnapshot()
    {
        return [.. _dict.Keys];
    }

    /// <summary>
    /// Checks if the current set is equal to another collection
    /// </summary>
    /// <param name="other">The collection to compare with</param>
    /// <returns>Returns true if both collections contain the same elements</returns>
    public bool SetEquals(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        var otherSet = other as ConcurrentSet<T> ?? new ConcurrentSet<T>();
        if (other is not ConcurrentSet<T>)
        {
            otherSet.AddRange(other);
        }
        else
        {
            otherSet = (ConcurrentSet<T>)other;
        }

        return Count == otherSet.Count && _dict.Keys.All(otherSet.Contains);
    }

    /// <summary>
    /// Checks if the current set is a subset of another collection
    /// </summary>
    /// <param name="other">The collection to check against</param>
    /// <returns>Returns true if the current set is a subset</returns>
    public bool IsSubsetOf(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        var otherSet = other.ToHashSet();
        return _dict.Keys.All(otherSet.Contains);
    }

    /// <summary>
    /// Checks if the current set is a superset of another collection
    /// </summary>
    /// <param name="other">The collection to check against</param>
    /// <returns>Returns true if the current set is a superset</returns>
    public bool IsSupersetOf(IEnumerable<T> other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        return other.All(Contains);
    }

    /// <summary>
    /// Returns a string representation of the set
    /// </summary>
    /// <returns>A string representation of the set</returns>
    public override string ToString()
    {
        return $"ConcurrentSet<{typeof(T).Name}> Count = {Count}";
    }
}