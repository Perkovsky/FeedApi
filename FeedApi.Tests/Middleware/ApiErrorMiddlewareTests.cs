using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using FeedApi.Extensions;

namespace FeedApi.Tests.Middleware
{
    /// <summary>
    /// Integration Tests.
    /// <para>
    /// Add to test project "Microsoft.AspNetCore.TestHost" using NuGet
    /// </para>
    /// </summary>
    [Trait("Middlewares", "ApiError")]
    public class ApiErrorMiddlewareTests
    {
        private readonly TestServer server;

        public ApiErrorMiddlewareTests()
        {
            // Arrange
            var builder = new WebHostBuilder()
                    .Configure(app =>
                    {
                        app.UseApiKeyErrorHandling();
                        app.Run(async context =>
                        {
                            await context.Response.WriteAsync("Test response");
                        });
                    }
            );
            this.server = new TestServer(builder);
        }

        [Fact]
        public async Task InvokeAsync_ReturnsHttpStatusCode_BadRequest()
        {
            // Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/api/1/CreateFeedCollection");
            var response = await server.CreateClient().SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ReturnsHttpStatusCode_Unauthorized()
        {
            // Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/api/1/CreateFeedCollection");
            request.Headers.Add("Authorization", "Incorrect API KEY");
            var response = await server.CreateClient().SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ReturnsHttpStatusCode_Ok()
        {
            // Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/api/1/CreateFeedCollection");
            request.Headers.Add("Authorization", "21B009CE-82BC-4F88-B656-154C476B89F1");
            var response = await server.CreateClient().SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
