using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Wyn.Quartz.Abstractions;
using Wyn.Utils.Interfaces;

namespace Wyn.Quartz.Core
{
    public class QuartzAppShutdownHandler : IAppShutdownHandler
    {
        private readonly IQuartzServer _server;
        private readonly ILogger _logger;

        public QuartzAppShutdownHandler(IQuartzServer server, ILogger<QuartzAppShutdownHandler> logger)
        {
            _server = server;
            _logger = logger;
        }

        public async Task Handle()
        {
            if (_server != null)
            {
                await _server.Stop();

                _logger.LogDebug("quartz server stop");
            }
        }
    }
}
