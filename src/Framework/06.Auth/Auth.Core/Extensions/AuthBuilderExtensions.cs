using System;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Wyn.Auth.Abstractions;
using Wyn.Auth.Abstractions.Options;
using Wyn.Auth.Jwt;

namespace Wyn.Auth.Core.Extensions
{
    public static class AuthBuilderExtensions
    {
        /// <summary>
        /// 使用Jwt身份认证方案
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static AuthBuilder UseJwt(this AuthBuilder builder, Action<JwtBearerOptions> configure = null)
        {
            var services = builder.Services;

            // 添加Jwt配置项
            var jwtOptions = new JwtOptions();
            builder.Configuration.GetSection("Wyn:Auth:Jwt").Bind(jwtOptions);
            services.AddSingleton(jwtOptions);

            // 添加凭证构造器
            //services.AddScoped<ICredentialBuilder, JwtCredentialBuilder>();
            services.AddTransient<ICredentialBuilder, JwtCredentialBuilder>();



            // 添加身份认证服务
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // 配置令牌验证参数
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key)),
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience
                    };

                    // 自定义配置
                    configure?.Invoke(options);
                });


            return builder;
        }
    }
}
