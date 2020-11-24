using System;
using System.Threading.Tasks;

namespace MS.GTA.ServicePlatform.Caching.Abstractions
{
    /// <summary>
    /// An interface for a cache in which any given item could be a different type than any other.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets the item from the cache or adds the value returned from the populating function.
        /// </summary>
        /// <typeparam name="T">The type of the object in the cache.</typeparam>
        /// <param name="key">The key of the cache entry.</param>
        /// <param name="populatingFunction">The function which provides the new value in the event of a cache miss.</param>
        /// <returns>The object on the cache.</returns>
        Task<T> GetOrAddValueAsync<T>(string key, Func<T> populatingFunction) where T : class;

        /// <summary>
        /// Gets the item from the cache or adds the value returned from the populating function.
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        /// <param name="populatingFunction">The function which provides the new value in the event of a cache miss.</param>
        /// <returns>The object on the cache.</returns>
        Task<T> GetOrAddValueAsync<T>(string key, Func<Task<T>> populatingFunction) where T : class;

        /// <summary>
        /// Gets an item of the cache if the cache has the value.
        /// </summary>
        /// <typeparam name="T">The type of the object in the cache.</typeparam>
        /// <param name="key">The key of the cache entry.</param>
        /// <returns>The object on the cache.</returns>
        Task<T> GetValueAsync<T>(string key) where T : class;

        /// <summary>
        /// Removes the specified value from the cache.
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Adds a value to the cache.
        /// </summary>
        /// <typeparam name="T">The type of the object in the cache.</typeparam>
        /// <param name="key">The key of the cache entry.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="cacheObjectLifetime">The initial lifetime of the object in cache.</param>
        Task SetAsync<T>(string key, T value, TimeSpan? cacheObjectLifetime = null) where T : class;
    }

    /// <summary>
    /// An interface for a cache in which all objects share a consistent type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICache<T>
    {
        /// <summary>
        /// Gets the item from the cache or adds the value returned from the populating function.
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        /// <param name="populatingFunction">The function which provides the new value in the event of a cache miss.</param>
        /// <returns>The object on the cache.</returns>
        Task<T> GetOrAddValueAsync(string key, Func<T> populatingFunction);

        /// <summary>
        /// Gets the item from the cache or adds the value returned from the populating function.
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        /// <param name="populatingFunction">The function which provides the new value in the event of a cache miss.</param>
        /// <returns>The object on the cache.</returns>
        Task<T> GetOrAddValueAsync(string key, Func<Task<T>> populatingFunction);

        /// <summary>
        /// Gets an item of the cache if the cache has the value.
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        /// <returns>The object on the cache.</returns>
        Task<T> GetValueAsync(string key);

        /// <summary>
        /// Removes the specified value from the cache.
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Adds a value to the cache.
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="cacheObjectLifetime">The initial lifetime of the object in cache.</param>
        Task SetAsync(string key, T value, TimeSpan? cacheObjectLifetime = null);
    }
}