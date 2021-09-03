﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Wyn.Module.Abstractions
{
    /// <summary>
    /// 模块服务配置器
    /// </summary>
    public interface IModuleServicesConfigurator
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="modules">模块列表</param>
        /// <param name="env">环境变量</param>
        /// <param name="cfg"></param>
        void Configure(IServiceCollection services, IModuleCollection modules, IHostEnvironment env, IConfiguration cfg);
    }
}
