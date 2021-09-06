using System;

using StackExchange.Redis;

using Wyn.Cache.Abstractions.Redis;
using Wyn.Utils.Extensions;
using Wyn.Utils.Helpers;

namespace Wyn.Cache.Core.Redis
{
    public class DefaultRedisSerializer : IRedisSerializer
    {
        public string Serialize<T>(T value) => IsNotBaseType<T>() ? JsonHelper.Serialize(value) : value.ToString();

        public T Deserialize<T>(RedisValue value) => IsNotBaseType<T>() ? JsonHelper.Deserialize<T>(value) : value.To<T>();

        public object Deserialize(RedisValue value, Type type) => Type.GetTypeCode(type) == TypeCode.Object ? JsonHelper.Deserialize(value, type) : value;

        /// <summary>
        /// 是否是基础类型
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <returns>真或假</returns>
        private static bool IsNotBaseType<T>() => Type.GetTypeCode(typeof(T)) == TypeCode.Object;
    }
}
