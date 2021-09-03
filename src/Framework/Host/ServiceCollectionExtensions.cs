using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;

using Wyz.Auth;
using Wyn.Cache;
using Wyn.Config;
using Wyn.Data;
using Wyn.Host.Abstractions;
using Wyn.Host.Core;
using Wyn.Mapper;
using Wyn.Module;
using Wyn.Module.Core;
using Wyn.Swagger;
using Wyn.Utils;
using Wyn.Utils.Attributes;

using HostOptions = Wyn.Host.Abstractions.HostOptions;

namespace Wyn.Host
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加WebHost
        /// </summary>
        /// <param name="services"></param>
        /// <param name="hostOptions"></param>
        /// <param name="env">环境</param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebHost(this IServiceCollection services, HostOptions hostOptions, IHostEnvironment env, IConfiguration cfg)
        {
            // 注入主机配置项
            services.AddSingleton(hostOptions);

            // 注入所有服务
            services.AddNetModularServices();

            // 加载缓存
            services.AddCache(cfg);

            // 加载模块
            var modules = services.AddModules();

            // 添加对象映射
            services.AddMappers(modules);

            // 主动或者开发模式下开启Swagger
            if (hostOptions.Swagger || env.IsDevelopment())
            {
                services.AddSwagger(modules);
            }

            services.AddControllers();

            // CORS
            services.AddCors(options =>
            {
                /*
                 *  浏览器的同源策略，就是出于安全考虑，浏览器会限制从脚本发起的跨域HTTP请求（比如异步请求GET, POST, PUT, DELETE, OPTIONS等等，
                 *  所以浏览器会向所请求的服务器发起两次请求，第一次是浏览器使用OPTIONS方法发起一个预检请求，第二次才是真正的异步请求，
                 *  第一次的预检请求获知服务器是否允许该跨域请求：如果允许，才发起第二次真实的请求；如果不允许，则拦截第二次请求。
                 *  Access-Control-Max-Age用来指定本次预检请求的有效期，单位为秒，，在此期间不用发出另一条预检请求。
                 */  
                var preflightMaxAge = hostOptions.PreflightMaxAge < 0 ? new TimeSpan(0, 30, 0) : new TimeSpan(0, 0, hostOptions.PreflightMaxAge);

                // 下载文件时，文件名称会保存在headers的Content-Disposition属性里面
                options.AddPolicy("Default",
                    builder => builder.AllowAnyOrigin()
                        .SetPreflightMaxAge(preflightMaxAge)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition"));
            });

            // 添加数据库，数据库依赖ILoginInfo，所以需要在添加身份认证以及MVC后添加数据库
            services.AddDb(cfg, modules);

            // 解决Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            // 添加HttpClient相关
            services.AddHttpClient();

            // 添加模块的自定义服务
            services.AddModuleServices(modules, env, cfg);

            // 添加配置管理
            services.AddConfig();

            // 身份认证(需要从配置中读取jwt相关配置，所以要放在配置管理后面)
            services.AddAuth();

            // 添加模块初始化服务
            services.AddModuleInitializerServices(modules, env, cfg);

            //// 添加Excel相关功能
            //services.AddExcel(cfg);

            //// 添加OSS相关功能
            //services.AddOSS(cfg);

            // 添加默认启动Logo
            services.AddSingleton<IStartLogoProvider, DefaultStartLogoProvider>();

            return services;
        }
    }
}
