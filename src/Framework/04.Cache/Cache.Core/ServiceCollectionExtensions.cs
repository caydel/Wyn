using Microsoft.Extensions.DependencyInjection;

using Wyn.Cache.Abstractions;
using Wyn.Cache.Core;
using Wyn.Cache.Core.MemoryCache;

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
    }
}