using System;
using System.Threading.Tasks;

using Wyn.Cache.Abstractions;

namespace Wyn.Cache.Redis
{
    public class RedisCacheHandler : ICacheHandler
    {
        private readonly RedisHelper _redis;

        public RedisCacheHandler(RedisHelper helper) => _redis = helper;

        public string Get(string key) => _redis.StringGetAsync<string>(key).GetAwaiter().GetResult();

        public T Get<T>(string key) => _redis.StringGetAsync<T>(key).GetAwaiter().GetResult();

        public Task<string> GetAsync(string key) => _redis.StringGetAsync<string>(key);

        public Task<T> GetAsync<T>(string key) => _redis.StringGetAsync<T>(key);

        public bool TryGetValue(string key, out string value)
        {
            value = null;
            if (Exists(key))
            {
                value = Get(key);
                return true;
            }

            return false;
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            value = default;
            if (Exists(key))
            {
                value = Get<T>(key);
                return true;
            }

            return false;
        }

        public bool Set<T>(string key, T value) => _redis.StringSetAsync(key, value).GetAwaiter().GetResult();

        public bool Set<T>(string key, T value, int expires) => _redis.StringSetAsync(key, value, new TimeSpan(0, 0, expires, 0)).GetAwaiter().GetResult();

        public Task<bool> SetAsync<T>(string key, T value) => _redis.StringSetAsync(key, value);

        public Task<bool> SetAsync<T>(string key, T value, int expires) => _redis.StringSetAsync(key, value, new TimeSpan(0, 0, expires, 0));

        public bool Remove(string key) => _redis.KeyDelete(key);

        public Task<bool> RemoveAsync(string key) => _redis.KeyDeleteAsync(key);

        public bool Exists(string key) => _redis.KeyExists(key);

        public Task<bool> ExistsAsync(string key) => _redis.KeyExistsAsync(key);

        public Task RemoveByPrefixAsync(string prefix) => _redis.DeleteByPrefix(prefix);
    }
}
