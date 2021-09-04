using System;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Wyn.Cache.Abstractions;
using Wyn.Cache.Abstractions.MemoryCache;
using Wyn.Cache.Core;
using Wyn.Cache.Options;
using Wyn.Cache.Redis;
using Wyn.Utils.Extensions;
using Wyn.Utils.Helpers;

namespace Wyn.Cache
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static CacheBuilder AddCache(this IServiceCollection services)
        {
            services.AddSingleton<ICacheHandler, MemoryCacheHandler>();

            return new CacheBuilder { Services = services };
        }

        /// <summary>
        /// 添加Redis缓存
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static CacheBuilder UseRedis(this CacheBuilder builder, IConfiguration cfg)
        {
            var services = builder.Services;

            services.Configure<RedisOptions>(cfg.GetSection("Mkh:Cache:Redis"));

            services.TryAddSingleton<IRedisSerializer, DefaultRedisSerializer>();

            services.AddSingleton<RedisHelper>();

            services.AddSingleton<ICacheHandler, RedisCacheHandler>();

            return builder;
        }
    }
}