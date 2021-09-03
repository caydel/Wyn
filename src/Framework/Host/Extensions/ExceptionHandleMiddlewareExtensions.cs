using Microsoft.AspNetCore.Builder;

using Wyn.Host.Core;

namespace Wyn.Host.Extensions
{
    public static class ExceptionHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandle(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandleMiddleware>();

            return app;
        }
    }
}
