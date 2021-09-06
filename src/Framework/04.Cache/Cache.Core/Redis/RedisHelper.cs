using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using StackExchange.Redis;

using Wyn.Cache.Abstractions.Options;
using Wyn.Cache.Abstractions.Redis;
using Wyn.Utils.Extensions;

namespace Wyn.Cache.Core.Redis
{
    public class RedisHelper : IDisposable
    {
        internal ConnectionMultiplexer _redis;
        internal string _prefix;
        internal readonly IOptionsMonitor<RedisOptions> _options;
        internal readonly IRedisSerializer _redisSerializer;
        public IDatabase Db { get; private set; }

        public RedisDatabase Database { get; private set; }

        public RedisHelper(IRedisSerializer redisSerializer, IOptionsMonitor<RedisOptions> options)
        {
            _redisSerializer = redisSerializer;
            _options = options;
            CreateConnection();
        }

        /// <summary>
        /// 获取Redis原生的IDatabase
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDb(int db = -1)
        {
            if (db == -1)
                db = _options.CurrentValue.DefaultDb;

            return _redis.GetDatabase(db);
        }

        /// <summary>
        /// 获取自定义的RedisDatabase
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public RedisDatabase GetDatabase(int db = -1)
        {
            if (db == -1)
                db = _options.CurrentValue.DefaultDb;

            return new RedisDatabase(db, this);
        }

        /// <summary>
        /// 获取键(附加前缀)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetKey(string key)
        {
            return $"{_prefix}{key}";
        }

        /// <summary>
        /// 获取Redis连接对象
        /// </summary>
        public ConnectionMultiplexer Conn
        {
            get
            {
                if (_redis == null)
                {
                    CreateConnection();
                }

                return _redis;
            }
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        internal void CreateConnection()
        {
            _prefix = _options.CurrentValue.KeyPrefix;
            _redis = ConnectionMultiplexer.Connect(_options.CurrentValue.ConnectionString);
            Db = GetDb();
            Database = GetDatabase();
        }

        #region String

        /// <summary>
        /// 写入字符串类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = null) => Database.StringSetAsync(key, obj, expiry);

        /// <summary>
        /// 获取字符串类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<T> StringGetAsync<T>(string key) => Database.StringGetAsync<T>(key);

        /// <summary>
        /// 字符串减去数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<long> StringDecrementAsync(string key, long value = 1) => Database.StringDecrementAsync(key, value);

        /// <summary>
        /// 字符串增加数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<long> StringIncrementAsync(string key, long value = 1) => Database.StringIncrementAsync(key, value);

        #endregion

        #region Hash

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Task<bool> HashSetAsync<T>(string key, string field, T obj) => Database.HashSetAsync(key, field, obj);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public Task<T> HashGetAsync<T>(string key, string field) => Database.HashGetAsync<T>(key, field);

        /// <summary>
        /// 获取所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<IList<T>> HashValuesAsync<T>(string key) => Database.HashValuesAsync<T>(key);

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public Task<bool> HashDeleteAsync(string key, string field) => Database.HashDeleteAsync(key, field);

        /// <summary>
        /// 获取所有键值集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<IList<KeyValuePair<string, T>>> HashGetAllAsync<T>(string key) => Database.HashGetAllAsync<T>(key);

        /// <summary>
        /// 数值减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<long> HashDecrementAsync(string key, string field, long value = 1) => Database.HashDecrementAsync(key, field, value);

        /// <summary>
        /// 数值加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<long> HashIncrementAsync(string key, string field, long value = 1) => Database.HashIncrementAsync(key, field, value);

        /// <summary>
        /// 判断字段是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public Task<bool> HashExistsAsync(string key, string field) => Database.HashExistsAsync(key, field);

        #endregion

        #region Set

        public Task<bool> SetAddAsync<T>(string key, T obj) => Database.SetAddAsync(key, obj);

        public Task<bool> SetRemoveAsync<T>(string key, T obj) => Database.SetRemoveAsync(key, obj);

        public Task<bool> SetContainsAsync<T>(string key, T obj) => Database.SetContainsAsync(key, obj);

        public Task<long> SetLengthAsync(string key) => Database.SetLengthAsync(key);

        public Task<T> SetPopAsync<T>(string key) => Database.SetPopAsync<T>(key);

        public Task<IList<T>> SetMembersAsync<T>(string key) => Database.SetMembersAsync<T>(key);

        #endregion

        #region Sorted Set

        /// <summary>
        /// 添加有序集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public Task<bool> SortedSetAddAsync<T>(string key, T member, double score) => Database.SortedSetAddAsync(key, member, score);

        /// <summary>
        /// 减去值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<double> SortedSetDecrementAsync<T>(string key, T member, double value) => Database.SortedSetDecrementAsync(key, member, value);

        /// <summary>
        /// 增加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<double> SortedSetIncrementAsync<T>(string key, T member, double value) => Database.SortedSetIncrementAsync(key, member, value);

        /// <summary>
        /// 获取排名的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Task<IList<T>> SortedSetRangeByRankAsync<T>(string key, long start = 0, long stop = -1, Order order = Order.Ascending) => Database.SortedSetRangeByRankAsync<T>(key, start, stop, order);

        /// <summary>
        /// 获取排名的成员和值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Task<IList<KeyValuePair<T, double>>> SortedSetRangeByRankWithScoresAsync<T>(string key, long start = 0, long stop = -1, Order order = Order.Ascending) => Database.SortedSetRangeByRankWithScoresAsync<T>(key, start, stop, order);

        /// <summary>
        /// 返回指定分数区间的成员数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Task<long> SortedSetLengthAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity) => Database.SortedSetLengthAsync(key, min, max);

        /// <summary>
        /// 删除并返回第一个元素，默认情况下，分数是从低到高排序的。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Task<KeyValuePair<T, double>> SortedSetPopAsync<T>(string key, Order order = Order.Ascending) => Database.SortedSetPopAsync<T>(key, order);

        /// <summary>
        /// 删除指定成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public Task<bool> SortedSetRemoveAsync<T>(string key, T member) => Database.SortedSetRemoveAsync(key, member);

        /// <summary>
        /// 根据分数区间删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns>删除数量</returns>
        public Task<long> SortedSetRemoveRangeByScoreAsync(string key, long start = 0, long stop = -1) => Database.SortedSetRemoveRangeByScoreAsync(key, start, stop);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns>删除数量</returns>
        public Task<long> SortedSetRemoveRangeByRankAsync(string key, long start = 0, long stop = -1) => Database.SortedSetRemoveRangeByRankAsync(key, start, stop);

        #endregion

        #region KeyDelete

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyDelete(string key) => Database.KeyDelete(key);

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> KeyDeleteAsync(string key) => Database.KeyDeleteAsync(key);

        #endregion

        #region KeyExists

        /// <summary>
        /// 是否存在键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyExists(string key) => Database.KeyExists(key);

        /// <summary>
        /// 是否存在键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> KeyExistsAsync(string key) => Database.KeyExistsAsync(key);

        #endregion

        #region KeyExpire

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, DateTime? expiry) => Database.KeyExpire(key, expiry);

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public Task<bool> KeyExpireAsync(string key, DateTime? expiry) => Database.KeyExpireAsync(key, expiry);

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expiry) => Database.KeyExpire(key, expiry);

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public Task<bool> KeyExpireAsync(string key, TimeSpan? expiry) => Database.KeyExpireAsync(key, expiry);

        #endregion

        #region Other

        /// <summary>
        /// 分页获取所有Keys
        /// </summary>
        /// <param name="database"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageOffset"></param>
        /// <returns></returns>
        public IList<RedisKey> GetAllKeys(int database = 0, int pageSize = 10, int pageOffset = 0) => _redis.GetServer(_redis.GetEndPoints()[0]).Keys(database, pageSize: pageSize, pageOffset: pageOffset).ToList();

        /// <summary>
        /// 分页获取指定前缀的Keys列表
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="database"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageOffset"></param>
        /// <returns></returns>
        public IList<RedisKey> GetKeysByPrefix(string prefix, int database = 0, int pageSize = 10, int pageOffset = 0) 
            => prefix.IsNull() ? null : _redis.GetServer(_redis.GetEndPoints()[0]).Keys(database, $"{GetKey(prefix)}*", pageSize, pageOffset).ToList();

        /// <summary>
        /// 删除指定前缀的Keys
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public Task DeleteByPrefix(string prefix) => Database.DeleteByPrefix(prefix);

        public void Dispose() => _redis?.Dispose();

        #endregion
    }
}
