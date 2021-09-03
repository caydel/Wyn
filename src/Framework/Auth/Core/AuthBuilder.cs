﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wyn.Auth.Core
{
    public class AuthBuilder
    {
        public IServiceCollection Services { get; set; }

        /// <summary>
        /// 配置属性
        /// </summary>
        public IConfiguration Configuration { get; set; }

        public AuthBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
        }
    }
}
