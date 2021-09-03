using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

using Wyn.Cache.Abstractions;
using Wyn.Utils.Extensions;

namespace Wyn.Cache.MemoryCache
{
    public class MemoryCacheHandler : ICacheHandler
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheHandler(IMemoryCache cache) => _cache = cache;

        public string Get(string key) => _cache.Get(key)?.ToString();

        public T Get<T>(string key) => _cache.Get<T>(key);

        public Task<string> GetAsync(string key) => Task.FromResult(Get(key));

        public Task<T> GetAsync<T>(string key) => Task.FromResult(_cache.Get<T>(key));


        public bool TryGetValue(string key, out string value) => _cache.TryGetValue(key, out value);

        public bool TryGetValue<T>(string key, out T value) => _cache.TryGetValue(key, out value);

        public bool Set<T>(string key, T value)
        {
            _cache.Set(key, value);
            return true;
        }

        public bool Set<T>(string key, T value, int expires)
        {
            _cache.Set(key, value, new TimeSpan(0, 0, expires, 0));
            return true;
        }

        public Task<bool> SetAsync<T>(string key, T value)
        {
            Set(key, value);
            return Task.FromResult(true);
        }

        public Task<bool> SetAsync<T>(string key, T value, int expires)
        {
            Set(key, value, expires);
            return Task.FromResult(true);
        }

        public bool Remove(string key)
        {
            _cache.Remove(key);
            return true;
        }

        public Task<bool> RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(true);
        }

        public bool Exists(string key) => TryGetValue(key, out _);

        public Task<bool> ExistsAsync(string key) => Task.FromResult(TryGetValue(key, out _));

        public async Task RemoveByPrefixAsync(string prefix)
        {
            if (prefix.IsNull())
                return;

            var keys = GetAllKeys().Where(m => m.StartsWith(prefix));
            foreach (var key in keys)
            {
                await RemoveAsync(key);
            }
        }

        private List<string> GetAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
    }
}
