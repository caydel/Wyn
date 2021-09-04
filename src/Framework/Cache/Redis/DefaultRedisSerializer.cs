using System;

using StackExchange.Redis;

using Wyn.Utils.Attributes;
using Wyn.Utils.Extensions;
using Wyn.Utils.Helpers;

namespace Wyn.Cache.Redis
{
    public class DefaultRedisSerializer : IRedisSerializer
    {
        public string Serialize<T>(T value)
        {
            if (IsNotBaseType<T>())
            {
                return JsonHelper.Serialize(value);
            }

            return value.ToString();
        }

        public T Deserialize<T>(RedisValue value)
        {
            if (IsNotBaseType<T>())
            {
                return JsonHelper.Deserialize<T>(value);
            }

            return value.To<T>();
        }

        public object Deserialize(RedisValue value, Type type)
        {
            if (Type.GetTypeCode(type) == TypeCode.Object)
            {
                return JsonHelper.Deserialize(value, type);
            }

            return value;
        }

        /// <summary>
        /// 是否是基础类型
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <returns>真或假</returns>
        private static bool IsNotBaseType<T>() => Type.GetTypeCode(typeof(T)) == TypeCode.Object;
    }
}
