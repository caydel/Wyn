using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Wyn.Utils.Helpers;
using Wyn.Utils.Result;

namespace Wyn.Host.Core
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionHandleMiddleware> _logger;
        public ExceptionHandleMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionHandleMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = _env.IsDevelopment() ? exception.ToString() : exception.Message;

            _logger.LogError(message: error);

            return context.Response.WriteAsync(JsonHelper.Serialize(ResultModel.Failed(error)));
        }
    }
}
