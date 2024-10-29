namespace Lemon.ModuleNavigation
{
    public static class ListKeyValuePairExtensions
    {
        public static bool ContainsKey(this List<KeyValuePair<string, object>> list, string key)
            => !string.IsNullOrEmpty(key) && list.Any(kvp => string.Equals(kvp.Key, key, StringComparison.OrdinalIgnoreCase));

        public static T? GetValue<T>(this List<KeyValuePair<string, object>> list, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var kvp = list.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
            if (kvp.Key is null)
                throw new KeyNotFoundException($"Key was not found: {key}");

            try
            {
                return kvp.Value switch
                {
                    null => default,
                    string s when typeof(T).IsEnum => (T)Enum.Parse(typeof(T), s, true),
                    _ when typeof(T) == typeof(string) => (T)(object)(kvp.Value.ToString() ?? string.Empty),
                    IConvertible convertible => (T)Convert.ChangeType(convertible, typeof(T)),
                    T typedValue => typedValue,
                    _ => throw new InvalidCastException($"Cannot convert value of type '{kvp.Value.GetType().Name}' to {typeof(T).Name}")
                };
            }
            catch (Exception ex) when (ex is not InvalidCastException)
            {
                throw new InvalidCastException(
                    $"Cannot convert the value '{kvp.Value}' of '{key}' to {typeof(T).Name}", ex);
            }
        }

        public static IEnumerable<T> GetValues<T>(this List<KeyValuePair<string, object>> list, string key) where T : notnull
        {
            if (string.IsNullOrEmpty(key))
                return [];

            return list.Where(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase))
                      .Select(x => x.Value switch
                      {
                          null => throw new InvalidDataException($"The value of '{x.Key}' is null!"),
                          string s when typeof(T).IsEnum => (T)Enum.Parse(typeof(T), s, true),
                          _ when typeof(T) == typeof(string) => (T)(object)(x.Value.ToString() ?? string.Empty),
                          IConvertible convertible => (T)Convert.ChangeType(convertible, typeof(T)),
                          T typedValue => typedValue,
                          _ => throw new InvalidCastException($"Cannot convert value of type '{x.Value.GetType().Name}' to {typeof(T).Name}")
                      });
        }

        public static bool TryGetValue<T>(this List<KeyValuePair<string, object>> list, string key, out T? value)
        {
            value = default;

            if (string.IsNullOrEmpty(key))
                return false;

            var kvp = list.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
            if (kvp.Key is null)
                return false;

            try
            {
                value = kvp.Value switch
                {
                    null => default,
                    string s when typeof(T).IsEnum => (T)Enum.Parse(typeof(T), s, true),
                    _ when typeof(T) == typeof(string) => (T)(object)(kvp.Value.ToString() ?? string.Empty),
                    IConvertible convertible => (T)Convert.ChangeType(convertible, typeof(T)),
                    T typedValue => typedValue,
                    _ => default
                };
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }
    }
}
