using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Logging
{
    /// <summary>
    /// Logging middleware.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(Headers.CorrelationId, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString("N");
                context.Request.Headers.TryAdd(Headers.CorrelationId, correlationId);
            }

            if (!CorrelationContext.IsSpecified)
            {
                CorrelationContext.CorrelationId = correlationId;
            }

            // Applies the correlation ID to the response header for client side tracking.
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.TryAdd(Headers.CorrelationId, correlationId);
                return Task.CompletedTask;
            });

            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogInformation(
                    "Request {method} {url} => {statusCode} {correlationId}",
                    context.Request.Method,
                    context.Request.Path.Value,
                    context.Response.StatusCode,
                    correlationId);
            }
        }
    }
}