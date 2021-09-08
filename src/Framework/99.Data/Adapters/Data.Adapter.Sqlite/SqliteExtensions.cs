using System;

using Wyn.Data.Abstractions;
using Wyn.Data.Abstractions.Adapter;
using Wyn.Data.Abstractions.Options;
using Wyn.Data.Core.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 使用Sqlite数据库
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="configure">自定义配置</param>
        /// <returns></returns>
        public static IDbBuilder UseSqlite(this IDbBuilder builder, string connectionString, Action<DbOptions> configure = null)
        {
            builder.UseDb(connectionString, DbProvider.Sqlite, configure);

            return builder;
        }
    }
}
