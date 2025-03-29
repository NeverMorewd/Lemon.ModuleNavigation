namespace Lemon.ModuleNavigation.Abstractions;

public interface IDialogParameters
{
    /// <summary>
    /// The number of parameters in the collection.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// The keys in the collection.
    /// </summary>
    IEnumerable<string> Keys { get; }

    /// <summary>
    /// Adds the key and value to the collection.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void Add(string key, object value);

    /// <summary>
    /// Checks the collection for the presence of a key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool ContainsKey(string key);

    /// <summary>
    /// Gets the parameter value referenced by a key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T? GetValue<T>(string key) where T : notnull;

    /// <summary>
    /// Gets all parameter values referenced by a key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    IEnumerable<T> GetValues<T>(string key) where T : notnull;

    /// <summary>
    /// Gets the parameter value if the referenced key exists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool TryGetValue<T>(string key, out T? value) where T : notnull;
}
