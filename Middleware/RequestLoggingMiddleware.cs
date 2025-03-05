using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoffeeShopWebAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request details *before* processing.
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} {context.Request.QueryString}");

            // Call the next middleware in the pipeline
            await _next(context);

            // Log the response details *after* processing.
            _logger.LogInformation($"Response: {context.Response.StatusCode}");
        }
    }
}