using Microsoft.Extensions.DependencyInjection;

namespace Wyn.Cache.Core
{
    /// <summary>
    /// 缓存构造器
    /// </summary>
    public class CacheBuilder
    {
        /// <summary>
        /// 服务
        /// </summary>
        public IServiceCollection Services { get; set; }
    }
}
