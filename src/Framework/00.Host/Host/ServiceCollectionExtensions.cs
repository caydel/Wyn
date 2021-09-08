using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;

using Data.Core;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Wyn.Auth;
using Wyn.Auth.Core;
using Wyn.Auth.Core.Extensions;
using Wyn.Cache;
using Wyn.Cache.Core.Extensions;
using Wyn.Data.Abstractions.Adapter;
using Wyn.Data.Core;
using Wyn.Data.Core.Extensions;
using Wyn.Host.Web.Swagger;
using Wyn.Host.Web.Swagger.Conventions;
using Wyn.Mapper.Core;
using Wyn.Module.Abstractions;
using Wyn.Module.Abstractions.Options;
using Wyn.Module.Core;
using Wyn.Module.Web;
using Wyn.MQ.Core;
using Wyn.Utils.Extensions;
using Wyn.Utils.Helpers;

using HostOptions = Wyn.Host.Web.Options.HostOptions;

namespace Wyn.Host.Web
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加模块核心功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IModuleCollection AddModules(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
        {
            var moduleOptionsList = configuration.Get<List<ModuleOptions>>("Wyn:Modules");

            var modules = new ModuleCollection(environment);
            modules.Load(moduleOptionsList);

            services.AddSingleton<IModuleCollection>(modules);

            return modules;
        }

        /// <summary>
        /// 添加Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules">模块集合</param>
        /// <param name="hostOptions"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IModuleCollection modules, HostOptions hostOptions, IHostEnvironment env)
            => services.AddSwagger(modules, hostOptions);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration cfg)
        {
            var builder = services.AddCache();

            var provider = cfg["Wyn:Cache:Provider"].ToInt();
            if (provider == 1)
            {
                builder.UseRedis(cfg);
            }

            return services;
        }

        /// <summary>
        /// 添加对象映射
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules">模块集合</param>
        /// <param name="env">模块集合</param>
        /// <returns></returns>
        public static IServiceCollection AddMappers(this IServiceCollection services, IModuleCollection modules, IHostEnvironment env)
            => services.AddMappers(modules);

        /// <summary>
        /// 添加MVC功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IServiceCollection AddMvc(this IServiceCollection services, IModuleCollection modules, Options.HostOptions hostOptions, IHostEnvironment env)
        {
            services.AddMvc(c =>
            {
                if (hostOptions!.Swagger || env.IsDevelopment())
                {
                    // API分组约定
                    c.Conventions.Add(new ApiExplorerGroupConvention());
                }
            })
                .AddJsonOptions(options =>
                {
                    // 不区分大小写的反序列化
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    // 属性名称使用 camel 大小写
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    // 最大限度减少字符转义
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                    // 添加日期转换器
                    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                })
                // 添加模块
                .AddModules(modules);

            return services;
        }

        /// <summary>
        /// 添加CORS
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hostOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddCors(this IServiceCollection services, Options.HostOptions hostOptions)
        {
            services.AddCors(options =>
            {
                /* 浏览器的同源策略，就是出于安全考虑，浏览器会限制从脚本发起的跨域HTTP请求（比如异步请求GET, POST, PUT, DELETE, OPTIONS等等，
                   所以浏览器会向所请求的服务器发起两次请求，第一次是浏览器使用OPTIONS方法发起一个预检请求，第二次才是真正的异步请求，
                   第一次的预检请求获知服务器是否允许该跨域请求：如果允许，才发起第二次真实的请求；如果不允许，则拦截第二次请求。
                   Access-Control-Max-Age用来指定本次预检请求的有效期，单位为秒，，在此期间不用发出另一条预检请求。*/
                var preflightMaxAge = hostOptions.PreflightMaxAge > 0 ? new TimeSpan(0, 0, hostOptions.PreflightMaxAge) : new TimeSpan(0, 30, 0);

                options.AddPolicy("Default",
                    builder => builder.SetIsOriginAllowed(_ => true)
                        .SetPreflightMaxAge(preflightMaxAge)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition"));//下载文件时，文件名称会保存在headers的Content-Disposition属性里面
            });

            return services;
        }

        /// <summary>
        /// 添加模块相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IServiceCollection AddModuleServices(this IServiceCollection services, IModuleCollection modules, IHostEnvironment env)
            => services.AddModuleServices(modules);

        /// <summary>
        /// 添加身份认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置属性</param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static AuthBuilder AddAuth(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env) 
            => services.AddAuth(configuration)?.UseJwt();

        /// <summary>
        /// 添加身份认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置属性</param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IServiceCollection AddMQ(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
            => services.AddRabbitMQ(configuration);

        /// <summary>
        /// 添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IServiceCollection AddData(this IServiceCollection services, IModuleCollection modules)
        {
            foreach (var module in modules)
            {
                var dbOptions = module.Options!.Db;
                var dbContextType = module.LayerAssemblies.Core?.GetTypes().FirstOrDefault(m => typeof(DbContext).IsAssignableFrom(m));

                var dbBuilder = services.AddDb(dbContextType, opt =>
                {
                    opt.Provider = dbOptions.Provider;

                    // Sqlite数据库自动创建数据库文件
                    if (dbOptions.ConnectionString.IsNull() && dbOptions.Provider == DbProvider.Sqlite)
                    {
                        string dbFilePath = Path.Combine(AppContext.BaseDirectory, "db");
                        if (!Directory.Exists(dbFilePath))
                            Directory.CreateDirectory(dbFilePath);
                        dbOptions.ConnectionString = $"Data Source={dbFilePath}/{module.Code}.db;Mode=ReadWriteCreate";
                    }

                    opt.ConnectionString = dbOptions.ConnectionString;
                    opt.Log = dbOptions.Log;
                    opt.TableNamePrefix = dbOptions.TableNamePrefix;
                    opt.TableNameSeparator = dbOptions.TableNameSeparator;
                    opt.Version = dbOptions.Version;
                });

                // 加载仓储
                dbBuilder.AddRepositoriesFromAssembly(module.LayerAssemblies.Core);

                // 启用代码优先
                if (dbOptions.CodeFirst)
                {
                    dbBuilder.AddCodeFirst(opt =>
                    {
                        opt.CreateDatabase = dbOptions.CreateDatabase;
                        opt.UpdateColumn = dbOptions.UpdateColumn;
                    });
                }

                // 特性事务
                foreach (var dic in module.ApplicationServices)
                {
                    dbBuilder.AddTransactionAttribute(dic.Key, dic.Value);
                }

                dbBuilder.Build();
            }

            return services;
        }

        
    }
}
