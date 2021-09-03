﻿using System;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Wyn.Module.Abstractions;
using Wyn.Module.Core;

namespace Wyn.Module
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IModuleCollection AddModules(this IServiceCollection services)
        {
            var modules = new ModuleCollection();
            modules.Load();

            services.AddSingleton<IModuleCollection>(modules);

            foreach (var module in modules)
            {
                if (module == null)
                    continue;

                services.AddApplicationServices(module);

                services.AddSingleton(module);
            }

            return modules;
        }

        /// <summary>
        /// 添加模块的自定义服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <param name="env"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IServiceCollection AddModuleServices(this IServiceCollection services, IModuleCollection modules, IHostEnvironment env, IConfiguration cfg)
        {
            foreach (var module in modules)
            {
                // 加载模块初始化器
                ((ModuleDescriptor)module).ServicesConfigurator?.Configure(services, modules, env, cfg);
            }

            return services;
        }

        /// <summary>
        /// 添加模块初始化服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules"></param>
        /// <param name="env"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IServiceCollection AddModuleInitializerServices(this IServiceCollection services, IModuleCollection modules, IHostEnvironment env, IConfiguration cfg)
        {
            foreach (var module in modules)
            {
                // 加载模块初始化器
                ((ModuleDescriptor)module).Initializer?.ConfigureServices(services, modules, env, cfg);
            }

            return services;
        }

        /// <summary>
        /// 添加应用服务
        /// </summary>
        private static void AddApplicationServices(this IServiceCollection services, IModuleDescriptor module)
        {
            if (module.AssemblyDescriptor == null)
                return;

            var types = module.AssemblyDescriptor.Application.GetTypes();
            var interfaces = types.Where(t => t.FullName != null && t.IsInterface && t.FullName.EndsWith("Service", StringComparison.OrdinalIgnoreCase));
            foreach (var serviceType in interfaces)
            {
                var implementationType = types.FirstOrDefault(m => m != serviceType && serviceType.IsAssignableFrom(m));
                if (implementationType != null)
                {
                    services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
                }
            }
        }
    }
}
