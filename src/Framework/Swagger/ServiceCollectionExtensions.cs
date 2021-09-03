using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using System.Collections.Generic;

using Wyn.Module;
using Wyn.Module.Core;
using Wyn.Swagger.Extensions;
using Wyn.Swagger.Filters;

namespace Wyn.Swagger
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules">模块集合</param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IModuleCollection modules)
        {
            _ = services.AddSwaggerGen(c =>
              {
                  if (modules != null)
                  {
                      foreach (var module in modules)
                      {
                          if (((ModuleDescriptor)module).Initializer == null)
                              continue;

                          foreach (var g in module.GetGroups())
                          {
                              c.SwaggerDoc(g.Key, new OpenApiInfo
                              {
                                  Title = g.Value,
                                  Version = module.Version
                              });
                          }
                      }
                  }

                  var securityScheme = new OpenApiSecurityScheme
                  {
                      Description = "JWT认证请求头格式: \"Authorization: Bearer {token}\"",
                      Name = "Authorization",
                      In = ParameterLocation.Header,
                      Type = SecuritySchemeType.ApiKey,
                      Scheme = "Bearer"
                  };

                // 添加设置Token的按钮
                c.AddSecurityDefinition("Bearer", securityScheme);

                // 添加Jwt验证设置
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                  });

                // 链接转小写过滤器
                c.DocumentFilter<LowercaseDocumentFilter>();

                // 描述信息处理
                c.DocumentFilter<DescriptionDocumentFilter>();

                // 隐藏属性
                c.SchemaFilter<IgnorePropertySchemaFilter>();
              });

            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }
    }
}
