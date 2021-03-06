using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Wyn.MQ.Abstractions.Options;

namespace Wyn.MQ.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加RabbitMQ功能
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration cfg)
        {
            var config = new RabbitMQOptions();
            var section = cfg.GetSection("Wyn:RabbitMQ");
            section?.Bind(config);

            services.AddSingleton(config);
            services.AddSingleton<RabbitMQClient>();

            return services;
        }
    }
}