using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Wyn.Host.Abstractions;

namespace Api
{
    public class Startup : StartupAbstract
    {
        public Startup(IHostEnvironment env, IConfiguration cfg) : base(env, cfg)
        {
        }
    }
}
