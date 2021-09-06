using System;
using System.Threading.Tasks;

using Wyn.Cache.Abstractions;

namespace Wyn.Cache.Core.Redis
{
    public class RedisCacheHandler : ICacheHandler
    {
        private readonly RedisHelper _redis;

        public RedisCacheHandler(RedisHelper helper) => _redis = helper;

        public Task<string> Get(string key) => _redis.StringGetAsync<string>(key);

        public Task<T> Get<T>(string key) => _redis.StringGetAsync<T>(key);

        public Task<bool> Set<T>(string key, T value) => _redis.StringSetAsync(key, value);

        public Task<bool> Set<T>(string key, T value, int expires) => _redis.StringSetAsync(key, value, new TimeSpan(0, 0, expires, 0));

        public Task<bool> Set<T>(string key, T value, DateTime expires) => _redis.StringSetAsync(key, value, expires - DateTime.Now);

        public Task<bool> Set<T>(string key, T value, TimeSpan expires) => _redis.StringSetAsync(key, value, expires);

        public Task<bool> Remove(string key) => _redis.KeyDeleteAsync(key);

        public Task<bool> Exists(string key) => _redis.KeyExistsAsync(key);

        public Task RemoveByPrefix(string prefix) => _redis.DeleteByPrefix(prefix);
    }
}
