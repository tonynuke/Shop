using System;
using System.Net.Http;
using Common.Hosting.Auth;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Prometheus;

namespace Common.Hosting
{
    /// <summary>
    /// <see cref="IServiceCollection"/> <see cref="HttpClient"/> extensions.
    /// </summary>
    public static class HttpClientCollectionExtensions
    {
        private const int RetryCount = 5;
        private const int HandlerLifeTimeMinutes = 5;

        /// <summary>
        /// Configures http client.
        /// </summary>
        /// <typeparam name="TClientInterface">Client interface.</typeparam>
        /// <typeparam name="TClientImplementation">Client implementation.</typeparam>
        /// <param name="services">Services collection.</param>
        /// <param name="endpointUrl">Endpoint url.</param>
        /// <returns>
        /// A builder for configuring named <see cref="HttpClient"/> instances returned by <see cref="IHttpClientFactory"/>.
        /// </returns>
        public static IHttpClientBuilder ConfigureHttpClient<TClientInterface, TClientImplementation>(
            this IServiceCollection services, string endpointUrl)
            where TClientInterface : class
            where TClientImplementation : class, TClientInterface
        {
            var retryPolicy = GetRetryPolicy();
            return services.AddHttpClient<TClientInterface, TClientImplementation>(
                    client => { client.BaseAddress = new Uri(endpointUrl); })
                .SetHandlerLifetime(TimeSpan.FromMinutes(HandlerLifeTimeMinutes))
                .AddPolicyHandler(retryPolicy)
                .AddHeaderPropagation()
                .UseHttpClientMetrics();
        }

        /// <summary>
        /// Configures http client.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <returns>
        /// A builder for configuring named <see cref="HttpClient"/> instances returned by <see cref="IHttpClientFactory"/>.
        /// </returns>
        public static IHttpClientBuilder ConfigureCrossServiceAuthHttpClient(
            this IServiceCollection services)
        {
            var retryPolicy = GetRetryPolicy();
            return services.AddHttpClient<HttpClient>(CrossServiceAuthHandler.Key)
                .SetHandlerLifetime(TimeSpan.FromMinutes(HandlerLifeTimeMinutes))
                .AddPolicyHandler(retryPolicy)
                .AddHeaderPropagation()
                .UseHttpClientMetrics();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var medianFirstRetryDelay = TimeSpan.FromMilliseconds(300);
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay, RetryCount);

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(message => message.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(delay);
        }
    }
}