using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Wyn.Host.Web.Middleware;
using Wyn.Host.Web.Swagger;
using Wyn.Module.Abstractions;
using Wyn.Module.Web;
using Wyn.Utils;
using Wyn.Utils.Extensions;

using HostOptions = Wyn.Host.Web.Options.HostOptions;

namespace Wyn.Host.Web
{
    public class Startup
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _cfg;

        public Startup(IHostEnvironment env, IConfiguration cfg)
        {
            _env = env;
            _cfg = cfg;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var hostOptions = services.GetService<HostOptions>();

            // 注入服务
            services.AddServicesFromAttribute();

            // 添加模块
            var modules = services.AddModules(_env, _cfg);

            // 添加Swagger
            services.AddSwagger(modules, hostOptions, _env);

            // 添加缓存
            services.AddCache(_cfg);

            // 添加对象映射
            services.AddMappers(modules,_env);

            // 添加MVC配置
            services.AddMvc(modules, hostOptions, _env);

            // 添加CORS
            services.AddCors(hostOptions);

            // 解决Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            // 添加HttpClient相关
            services.AddHttpClient();

            // 添加模块的自定义特有的服务
            services.AddModuleServices(modules, _env);

            // 添加身份认证和授权
            services.AddAuth(_cfg, _env);

            // 添加消息队列
            services.AddMQ(_cfg, _env);

            // 添加数据库
            services.AddData(modules);
        }

        public virtual void Configure(IApplicationBuilder app, IHostApplicationLifetime appLifetime)
        {
            var hostOptions = app.ApplicationServices.GetService<HostOptions>();
            var modules = app.ApplicationServices.GetRequiredService<IModuleCollection>();

            // 使用全局异常处理中间件
            app.UseMiddleware<ExceptionHandleMiddleware>();

            // 基地址
            var pathBase = hostOptions!.Base;
            if (pathBase.NotNull())
            {
                // 1、配置请求基础地址：
                app.Use((context, next) =>
                {
                    context.Request.PathBase = pathBase;
                    return next();
                });

                // 2、配置静态文件基地址：
                app.UsePathBase(pathBase);
            }

            // 反向代理
            if (hostOptions!.Proxy)
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            // 路由
            app.UseRouting();

            // CORS
            app.UseCors("Default");

            // 认证
            app.UseAuthentication();

            // 授权
            app.UseAuthorization();

            // 配置端点
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // 启用Swagger
            app.UseSwagger(modules, hostOptions);

            // 使用模块化
            app.UseModules(modules);

            appLifetime.ApplicationStarted.Register(() =>
            {
                // 显示启动Banner
                var options = app.ApplicationServices.GetService<HostOptions>();
                ConsoleBanner(options);
            });
        }

        /// <summary>
        /// 启动后打印Banner图案
        /// </summary>
        /// <param name="options"></param>
        private void ConsoleBanner(HostOptions options)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine(@" ******************************************************************************************************");
            Console.WriteLine(@" *                             .--.      .--.   ____     __ ,---.   .--.                              *");
            Console.WriteLine(@" *                             |  |_     |  |   \   \   /  /|    \  |  |                              *");
            Console.WriteLine(@" *                             | _( )_   |  |    \  _. /  ' |  ,  \ |  |                              *");
            Console.WriteLine(@" *                             |(_ o _)  |  |     _( )_ .'  |  |\_ \|  |                              *");
            Console.WriteLine(@" *                             | (_,_) \ |  | ___(_ o _)'   |  _( )_\  |                              *");
            Console.WriteLine(@" *                             |  |/    \|  ||   |(_,_)'    | (_ o _)  |                              *");
            Console.WriteLine(@" *                             |  '  /\  `  ||   `-'  /     |  (_,_)\  |                              *");
            Console.WriteLine(@" *                             |    /  \    | \      /      |  |    |  |                              *");
            Console.WriteLine(@" *                             `---'    `---`  `-..-'       '--'    '--'                              *");
            Console.WriteLine(@" *                                                                                                    *");
            Console.WriteLine(@" *                                                                                                    *");
            Console.Write(@" *                                      ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(@"启动成功，欢迎使用 Wyn ~");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"                                      *");
            Console.WriteLine(@" *                                                                                                    *");
            Console.Write(@" *                                      ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(@"接口地址：" + options.Urls);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"                                       *");
            Console.WriteLine(@" *                                                                                                    *");
            Console.WriteLine(@" ******************************************************************************************************");
            Console.WriteLine();
        }
    }
}
