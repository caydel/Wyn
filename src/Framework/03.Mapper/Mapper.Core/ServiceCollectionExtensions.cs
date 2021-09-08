using System;

using Microsoft.Extensions.DependencyInjection;

using AutoMapper;

using Wyn.Module.Abstractions;
using Wyn.Utils.Annotations;

using IMapper = Wyn.Mapper.Abstractions.IMapper;

namespace Wyn.Mapper.Core
{
    public static class ServiceCollectionExtensions
    {
         /// <summary>
        /// 添加对象映射
        /// </summary>
        /// <param name="services"></param>
        /// <param name="modules">模块集合</param>
        /// <returns></returns>
        public static IServiceCollection AddMappers(this IServiceCollection services, IModuleCollection modules)
        {
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var module in modules)
                {
                    var types = module.LayerAssemblies.Core?.GetTypes();
                    if (types == null) continue;
                    foreach (var type in types)
                    {
                        var map = (ObjectMapAttribute)Attribute.GetCustomAttribute(type, typeof(ObjectMapAttribute));
                        if (map != null)
                        {
                            cfg.CreateMap(type, map.TargetType);

                            if (map.TwoWay)
                            {
                                cfg.CreateMap(map.TargetType, type);
                            }
                        }
                    }
                }
            });

            services.AddSingleton(config.CreateMapper());
            services.AddSingleton<IMapper, DefaultMapper>();

            return services;
        }
    }
}
