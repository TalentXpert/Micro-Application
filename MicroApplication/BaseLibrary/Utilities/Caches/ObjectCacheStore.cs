using System.Collections.Concurrent;

namespace BaseLibrary.Utilities.Caches
{
    public class ObjectCacheStore
    {
        private class CacheItem
        {
            public object Value { get; }
            public DateTime ExpiryTime { get; }

            public CacheItem(object value, TimeSpan ttl)
            {
                Value = value;
                ExpiryTime = DateTime.UtcNow.Add(ttl);
            }

            public bool IsExpired => DateTime.UtcNow >= ExpiryTime;
        }

        static ConcurrentDictionary<string, CacheItem> store = new();
        public static void Store(string key, object value, TimeSpan? cleanUpInterval = null)
        {
            CleanUpExpiredItems();
            var interval = cleanUpInterval ?? TimeSpan.FromMinutes(10);
            store[key] = new CacheItem(value, interval);
        }
        
        public static object? Get(string key) 
        { 
            if(store.ContainsKey(key))
                return store[key].Value;
            return null;
        }
        private static void CleanUpExpiredItems()
        {
            foreach (var pair in store)
            {
                if (pair.Value.IsExpired)
                {
                    store.TryRemove(pair.Key, out _);
                }
            }
        }
    }

}
