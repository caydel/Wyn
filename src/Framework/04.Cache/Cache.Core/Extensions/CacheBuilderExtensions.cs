using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Wyn.Cache.Abstractions;
using Wyn.Cache.Abstractions.Options;
using Wyn.Cache.Abstractions.Redis;
using Wyn.Cache.Core.Redis;

namespace Wyn.Cache.Core.Extensions
{
    public static class CacheBuilderExtensions
    {
        /// <summary>
        /// 添加Redis缓存
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static CacheBuilder UseRedis(this CacheBuilder builder, IConfiguration cfg)
        {
            var services = builder.Services;

            services.Configure<RedisOptions>(cfg.GetSection("Wyn:Cache:Redis"));

            services.TryAddSingleton<IRedisSerializer, DefaultRedisSerializer>();

            services.AddSingleton<RedisHelper>();

            services.AddSingleton<ICacheHandler, RedisCacheHandler>();

            return builder;
        }
    }
}
