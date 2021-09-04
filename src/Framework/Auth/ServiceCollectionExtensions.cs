﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Wyn.Auth.Abstractions;
using Wyn.Auth.Core;
using Wyn.Auth.Options;
using Wyn.Data.Abstractions;

namespace Wyn.Auth
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加身份认证核心功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置属性</param>
        /// <returns></returns>
        public static AuthBuilder AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            // 添加身份认证配置项
            services.Configure<AuthOptions>(configuration.GetSection("Mkh:Auth"));

            // 添加http上下文访问器，用于获取认证信息
            services.AddHttpContextAccessor();

            // 尝试添加账户信息
            services.TryAddSingleton<IAccount, Account>();

            // 添加权限解析器
            services.AddSingleton<IPermissionResolver, PermissionResolver>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MKH", policy => policy.Requirements.Add(new PermissionRequirement()));
            });

            // 自定义权限验证处理器
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            // 添加数据访问的账户解析器实现
            services.AddSingleton<IAccountResolver, AccountResolver>();

            var builder = new AuthBuilder(services, configuration);

            return builder;
        }
    }
}
