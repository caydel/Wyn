using System;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Wyn.Cache.Abstractions;
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
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration cfg)
        {
            var config = new CacheConfig();
            var section = cfg.GetSection("Cache");
            section?.Bind(config);

            services.AddSingleton(config);

            // var assembly = AssemblyHelper.LoadByNameEndString($".Cache.{config.Provider.ToString()}");
            var assembly = AssemblyHelper.LoadByNameEndString($".Cache");

            if (!assembly.NotNull())
            {
                throw new ArgumentNullException($"缓存实现程序集({config.Provider})未找到",
                   $"缓存实现程序集({config.Provider})未找到");
            }

            var handlerType = assembly.GetTypes().FirstOrDefault(m => typeof(ICacheHandler).IsAssignableFrom(m));
            if (handlerType != null)
            {
                services.AddSingleton(typeof(ICacheHandler), handlerType);
            }

            return services;
        }
    }
}