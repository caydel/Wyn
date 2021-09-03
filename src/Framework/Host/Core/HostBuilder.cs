﻿using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Wyn.Host.Abstractions;
using Wyn.Log;
using Wyn.Utils.Extensions;

using HostOptions = Wyn.Host.Abstractions.HostOptions;

namespace Wyn.Host.Core
{
    /// <summary>
    /// WebHost构造器
    /// </summary>
    public class HostBuilder
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args">启动参数</param>
        public void Run<TStartup>(string[] args) where TStartup : StartupAbstract
        {
            CreateBuilder<TStartup>(args).Build().Run();
        }

        /// <summary>
        /// 创建主机生成器
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public IHostBuilder CreateBuilder<TStartup>(string[] args) where TStartup : StartupAbstract
        {

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false);

            var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environmentVariable.NotNull())
            {
                configBuilder.AddJsonFile($"appsettings.{environmentVariable}.json", false);
            }

            var config = configBuilder.Build();

            var hostOptions = new HostOptions();
            config.GetSection("Host").Bind(hostOptions);

            if (hostOptions.Urls.IsNull())
                hostOptions.Urls = "http://*:5000";

            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                    .UseDefaultServiceProvider(options => { options.ValidateOnBuild = false; })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseLogging()
                            .UseStartup<TStartup>()
                            .UseUrls(hostOptions.Urls);
                    });
           
        }
    }
}
