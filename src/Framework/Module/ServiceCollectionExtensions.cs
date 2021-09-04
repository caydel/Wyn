using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Wyn.Module;
using Wyn.Module.Abstractions;
using Wyn.Module.Core;
using Wyn.Module.Descriptor;
using Wyn.Module.Options;
using Wyn.Utils.Extensions;

namespace Wyn.Module
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
        public static IModuleCollection AddModulesCore(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
        {
            var moduleOptionsList = configuration.Get<List<ModuleOptions>>("Mkh:Modules");

            var modules = new ModuleCollection(environment);
            modules.Load(moduleOptionsList);

            services.AddSingleton<IModuleCollection>(modules);

            return modules;
        }

        /// <summary>
        /// 添加模块相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IServiceCollection AddModuleServices(this IServiceCollection services, IModuleCollection modules)
        {
            foreach (var module in modules)
            {
                if (module == null)
                    continue;

                // 加载模块初始化器
                module.ServicesConfigurator?.Configure(services, modules.HostEnvironment);

                services.AddApplicationServices(module);
            }

            return services;
        }

        /// <summary>
        /// 添加应用服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="module"></param>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, ModuleDescriptor module)
        {
            var assembly = module.LayerAssemblies.Core;
            // 按照约定，应用服务必须采用Service结尾
            var implementationTypes = assembly.GetTypes().Where(m => m.Name.EndsWith("Service") && !m.IsInterface).ToList();

            foreach (var implType in implementationTypes)
            {
                // 按照约定，服务的第一个接口类型就是所需的应用服务接口
                var serviceType = implType.GetInterfaces()[0];

                services.AddScoped(implType);

                module.ApplicationServices.Add(serviceType, implType);
            }

            return services;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleCollection AddModules(this IMvcBuilder builder, IModuleCollection modules)
        {
            foreach (var module in modules)
            {
                var assembly = module.LayerAssemblies.Web ?? module.LayerAssemblies.Api;
                if (assembly == null)
                    continue;

                if (module?.LayerAssemblies == null)
                    continue;

                builder.AddApplicationPart(module.LayerAssemblies.Web ?? module.LayerAssemblies.Api);

                builder.AddMvcOptions(options =>
                {
                    var mvcOptionsConfigurator = assembly.GetTypes().FirstOrDefault(m => typeof(IModuleMvcOptionsConfigurator).IsAssignableFrom(m));
                    if (mvcOptionsConfigurator != null)
                    {
                        ((IModuleMvcOptionsConfigurator)Activator.CreateInstance(mvcOptionsConfigurator))?.Configure(options);
                    }
                });
            }

            return modules;
        }

        /// <summary>
        /// 使用模块的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="modules">模块集合</param>
        /// <returns></returns>
        public static IApplicationBuilder UseModules(this IApplicationBuilder app, IModuleCollection modules)
        {

            foreach (var module in modules)
            {
                if (module?.LayerAssemblies == null)
                    continue;

                var assembly = module.LayerAssemblies.Web ?? module.LayerAssemblies.Api;
                if (assembly == null)
                    continue;

                var middlewareConfigurator = assembly.GetTypes().FirstOrDefault(m => typeof(IModuleMiddlewareConfigurator).IsAssignableFrom(m));
                if (middlewareConfigurator != null)
                {
                    ((IModuleMiddlewareConfigurator)Activator.CreateInstance(middlewareConfigurator))?.Configure(app);
                }
            }

            return app;
        }
    }
}
