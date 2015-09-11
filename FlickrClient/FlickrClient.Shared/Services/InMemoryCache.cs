using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrClient.Services
{
    public class InMemoryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly object _sync = new object();

        private readonly Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        #region Implementation of ICache<TKey,TValue>

        /// <summary>
        /// Returns copy of all cached <see cref="KeyValuePair{TKey,TValue}">items</see> as <see cref="IEnumerable{T}">enumeration</see>.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{T}">Enumeration</see> of all cached items as <see cref="KeyValuePair{TKey,TValue}">pairs of key and value</see>.
        /// </returns>
        public IEnumerable<KeyValuePair<TKey, TValue>> GetAll()
        {
            return new List<KeyValuePair<TKey, TValue>>(_cache);
        }

        /// <summary>
        /// Initializes cache with predefined set of <see cref="KeyValuePair{TKey,TValue}">key-value pairs</see>.
        /// Exceptions:
        ///     <exception cref="ArgumentNullException">ArgumentNullException</exception>.
        ///     <exception cref="ArgumentException">ArgumentException</exception>.
        /// </summary>
        /// <param name="items">
        /// <see cref="IEnumerable{T}">Enumeration</see> of all items as <see cref="KeyValuePair{TKey,TValue}">pairs of key and value</see>.
        /// </param>
        public void Initialize(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            lock (_sync)
            {
                _cache.Clear();

                if (items == null) return;

                foreach (var pair in items)
                {
                    _cache.Add(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Determines whether cache contains key.
        /// Exceptions:
        ///     <exception cref="ArgumentNullException">ArgumentNullException</exception>.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns>True or false.</returns>
        public bool Contains(TKey key)
        {
            return _cache.ContainsKey(key);
        }

        /// <summary>
        /// Tries to get value by key.
        /// Exceptions:
        ///     <exception cref="ArgumentNullException">ArgumentNullException</exception>.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value parameter to be filled.</param>
        /// <returns>True if success, otherwise false.</returns>
        public bool TryGet(TKey key, out TValue value)
        {
            return _cache.TryGetValue(key, out value);
        }

        /// <summary>
        /// Get value by key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            TValue value = default(TValue);
            if (_cache.ContainsKey(key))
                value = _cache[key];
            return value;
        }

        /// <summary>
        /// Tries to put key-value pair into cache.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// Exceptions:
        ///     <exception cref="ArgumentNullException">ArgumentNullException</exception>.
        ///     <exception cref="ArgumentException">ArgumentException</exception>.
        /// <returns>True if success, otherwise false.</returns>
        public bool TryPut(TKey key, TValue value)
        {
            try
            {
                lock (_sync)
                {
                    if (!_cache.ContainsKey(key))
                    {
                        _cache.Add(key, value);
                    }
                    else
                    {
                        _cache[key] = value;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
               // Diagnostics.Debug.WriteLine(ex);
            }

            return false;
        }

        /// <summary>
        /// Removes all items from underlying <see cref="Dictionary{TKey,TValue}">dictionary</see>.
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        #endregion
    }
}
