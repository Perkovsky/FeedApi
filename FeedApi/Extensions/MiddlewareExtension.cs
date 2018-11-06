using Microsoft.AspNetCore.Builder;
using FeedApi.Middlewares;

namespace FeedApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiKeyErrorHandlingMiddleware>();
        }
    }
}