using Microsoft.Extensions.Caching.Memory;

namespace Core.Cache
{
    public class CacheManager
    {
        private readonly IMemoryCache _cache;

        public CacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set<T>(string key, T obj)
        {
            lock (_cache)
            {
                _cache.Set(key, obj);
            }
        }

        public T Get<T>(string key)
        {
            lock (_cache)
            {
                bool exist = _cache.TryGetValue(key, out T cachedItem);
                if (exist) return cachedItem;
                else
                    return default(T);
            }
        }

        public bool Exist<T>(string key)
        {
            bool exist = _cache.TryGetValue(key, out T cachedItem);
            return exist;
        }

        public void Remove<T>(T obj)
        {
            lock (_cache)
            {
                _cache.Remove(obj);
            }
        }

    }
}
