using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FeedApi.Middlewares
{
    public class ApiKeyErrorHandlingMiddleware
    {
        private readonly string API_KEY = "21B009CE-82BC-4F88-B656-154C476B89F1";

        private RequestDelegate nextDelegate;

        public ApiKeyErrorHandlingMiddleware(RequestDelegate nextDelegate) => this.nextDelegate = nextDelegate;

        private bool IsValidApiKey(string apiKey) => apiKey == API_KEY;

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (request.Path.StartsWithSegments(new PathString("/api")))
            {
                if (request.Headers["Authorization"].Any())
                {
                    string apiKey = request.Headers["Authorization"].FirstOrDefault();
                    if (IsValidApiKey(apiKey))
                    {
                        await nextDelegate.Invoke(context);
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await response.WriteAsync("Access Denied. Invalid Api key", Encoding.UTF8);
                    }
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await response.WriteAsync("Api key is misssing", Encoding.UTF8);
                }
            }
            else
            {
                await nextDelegate.Invoke(context);
            }
        }
    }
}
