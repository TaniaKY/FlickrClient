using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrClient.Services
{
    public interface ICache<TKey, TValue>
    {
        IEnumerable<KeyValuePair<TKey, TValue>> GetAll();
        void Initialize(IEnumerable<KeyValuePair<TKey, TValue>> items);
        bool Contains(TKey key);
        bool TryGet(TKey key, out TValue value);
        TValue Get(TKey key);
        bool TryPut(TKey key, TValue value);
        void Clear();
    }
}
