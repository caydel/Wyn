using Microsoft.Extensions.Configuration;

using System;
using System.IO;

using Wyn.Utils.Attributes;
using Wyn.Utils.Extensions;

namespace Wyn.Utils.Helpers
{

    /// <summary>
    /// 配置帮助类
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// 根据配置名称加载指定的配置项
        /// </summary>
        /// <param name="configFileName">配置文件名称，使用约定，配置文件放在项目的config目录中，如logging配置：config/logging.json</param>
        /// <param name="environmentName"></param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static IConfiguration Load(string configFileName, string environmentName = "", bool reloadOnChange = false)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "config");
            if (!Directory.Exists(filePath))
                return null;

            var builder = new ConfigurationBuilder()
                .SetBasePath(filePath)
                .AddJsonFile(configFileName.ToLower() + ".json", true, reloadOnChange);

            if (environmentName.NotNull())
                builder.AddJsonFile(configFileName.ToLower() + "." + environmentName + ".json", true, reloadOnChange);

            return builder.Build();
        }

        /// <summary>
        /// 根据配置名称加载指定的配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configFileName">配置文件名称，使用约定，配置文件放在项目的config目录中，如logging配置：config/logging.json</param>
        /// <param name="environmentName"></param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static T Get<T>(string configFileName, string environmentName = "", bool reloadOnChange = false)
        {
            var configuration = Load(configFileName, environmentName, reloadOnChange);
            return configuration == null ? default : configuration.To<T>();
        }
    }
}
