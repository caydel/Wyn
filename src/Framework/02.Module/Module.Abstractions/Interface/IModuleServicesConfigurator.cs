using Microsoft.Extensions.Configuration;
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
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        void Configure(IServiceCollection services, IHostEnvironment environment);
    }
}
