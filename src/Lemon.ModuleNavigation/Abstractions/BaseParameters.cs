using Lemon.ModuleNavigation.Extensions;
using System.Collections;
using System.ComponentModel;
using System.Text;

namespace Lemon.ModuleNavigation.Abstractions;

public abstract class BaseParameters : IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
    private readonly List<KeyValuePair<string, object>> _entries = [];
    public BaseParameters()
    {

    }
    public object? this[string key]
    {
        get
        {
            foreach (KeyValuePair<string, object> entry in _entries)
            {
                if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                {
                    return entry.Value;
                }
            }

            return null;
        }
    }
    public int Count => _entries.Count;
    public IEnumerable<string> Keys => _entries.Select((x) => x.Key);

    public void Add(string key, object value)
    {
        _entries.Add(new KeyValuePair<string, object>(key, value));
    }
    public bool ContainsKey(string key)
    {
        return _entries.ContainsKey(key);
    }
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _entries.GetEnumerator();
    }
    public T? GetValue<T>(string key) where T : notnull
    {
        return _entries.GetValue<T>(key);
    }
    public IEnumerable<T> GetValues<T>(string key) where T : notnull
    {
        return _entries.GetValues<T>(key);
    }
    public bool TryGetValue<T>(string key, out T? value) where T : notnull
    {
        return _entries.TryGetValue(key, out value);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        if (_entries.Count > 0)
        {
            stringBuilder.Append('?');
            bool flag = true;
            foreach (KeyValuePair<string, object> entry in _entries)
            {
                if (!flag)
                {
                    stringBuilder.Append('&');
                }
                else
                {
                    flag = false;
                }

                stringBuilder.Append(Uri.EscapeDataString(entry.Key));
                stringBuilder.Append('=');
                stringBuilder.Append(Uri.EscapeDataString(entry.Value != null ? entry.Value.ToString()! : ""));
            }
        }

        return stringBuilder.ToString();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void FromParameters(IEnumerable<KeyValuePair<string, object>> parameters)
    {
        _entries.AddRange(parameters);
    }
}
