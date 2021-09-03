﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Wyn.Host.Extensions;
using Wyn.Utils.Extensions;

namespace Wyn.Host.Abstractions
{
    public abstract class StartupAbstract
    {
        protected readonly IHostEnvironment Env;
        private readonly IConfiguration _cfg;
        private readonly HostOptions _hostOptions;

        protected StartupAbstract(IHostEnvironment env, IConfiguration cfg)
        {
            Env = env;
            _cfg = cfg;

            // 主机配置
            _hostOptions = new HostOptions();
            cfg.GetSection("Host").Bind(_hostOptions);

            if (_hostOptions.Urls.IsNull())
                _hostOptions.Urls = "http://*:5000";
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddWebHost(_hostOptions, Env, _cfg);
        }

        public virtual void Configure(IApplicationBuilder app, IHostApplicationLifetime appLifetime)
        {
            app.UseWebHost(_hostOptions, Env);

            app.UseShutdownHandler();

            appLifetime.ApplicationStarted.Register(() =>
            {
                // 显示启动Logo
                app.ApplicationServices.GetService<IStartLogoProvider>().Show(_hostOptions);
            });
        }
    }
}
