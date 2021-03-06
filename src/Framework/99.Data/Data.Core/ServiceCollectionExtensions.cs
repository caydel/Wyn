using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Wyn.Data.Abstractions;
using Wyn.Data.Abstractions.Adapter;
using Wyn.Data.Abstractions.Logger;
using Wyn.Data.Abstractions.Options;
using Wyn.Data.Core;
using Wyn.Data.Core.Internal;
using Wyn.Utils.Helpers;

namespace Data.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加数据库核心
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">自定义配置项委托</param>
        /// <returns></returns>
        public static IDbBuilder AddDb<TDbContext>(this IServiceCollection services, Action<DbOptions> configure = null)
            where TDbContext : IDbContext 
            => services.AddDb(typeof(TDbContext), configure);

        /// <summary>
        /// 添加数据库核心功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbContextType">数据库上下文类型</param>
        /// <param name="configure">自定义配置项委托</param>
        /// <returns></returns>
        public static IDbBuilder AddDb(this IServiceCollection services, Type dbContextType, Action<DbOptions> configure = null)
        {
            var options = new DbOptions();

            configure?.Invoke(options);

            if (options.Provider != DbProvider.Sqlite)
                GenericHelper.NotNull(options.ConnectionString, "连接字符串未配置");

            // 添加仓储实例管理器
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            // 尝试添加默认账户信息解析器
            services.TryAddSingleton<IAccountResolver, DefaultAccountResolver>();

            // 尝试添加默认的数据库操作日志记录器
            services.TryAddSingleton<IDbLoggerProvider, DbLoggerProvider>();

            return new DbBuilder(services, options, dbContextType);
        }
    }
}
