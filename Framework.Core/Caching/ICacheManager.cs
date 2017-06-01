using System;

namespace Framework.Core.Caching
{
    /// <summary>
    /// Cache manager interface
    /// </summary>
    public interface ICacheManager : IDisposable
    {

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// Add the specified keys and values to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">value</param>
        /// <param name="cacheTime">Specify expiration time</param>
        void Set(string key, object data, DateTime cacheTime);

        /// <summary>
        /// Gets a value that indicates whether the key is associated with the key.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>result</returns>
        bool Any(string key);

        /// <summary>
        /// Remove the specified key and value from the cache
        /// </summary>
        /// <param name="key">key</param>
        void Remove(string key);

        /// <summary>
        /// Clear all cache
        /// </summary>
        void Clear();

    }
}
