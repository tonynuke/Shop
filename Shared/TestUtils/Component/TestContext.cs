using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit.Abstractions;

namespace TestUtils.Component
{
    /// <summary>
    /// Test context.
    /// </summary>
    /// <typeparam name="TStartup">Startup.</typeparam>
    public class TestContext<TStartup> : IDisposable
        where TStartup : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestContext{TStartup}"/> class.
        /// </summary>
        public TestContext()
        {
            int port = WireMockServer.Ports.Single();

            using var stream = AppSettingsHelper.GetOverridenAppSettings(port);
            Configuration = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .AddEnvironmentVariables()
                .Build();

            Factory = new WebApplicationFactory<TStartup>()
                .WithWebHostBuilder(s =>
                    s.ConfigureAppConfiguration(builder =>
                        {
                            builder.Sources.Clear();
                            builder.AddConfiguration(Configuration);
                        })
                        .ConfigureLogging(builder =>
                        {
                            // TODO: create test context per test if you need logging.
                            builder.ClearProviders();
                        }));
        }

        /// <summary>
        /// Gets configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets web application factory.
        /// </summary>
        public WebApplicationFactory<TStartup> Factory { get; }

        /// <summary>
        /// Gets wireMock server.
        /// </summary>
        public WireMockServer WireMockServer { get; } = WireMockServer.Start();

        /// <inheritdoc/>
        public void Dispose()
        {
            WireMockServer.Stop();
            WireMockServer.Dispose();

            Factory.Dispose();
        }

        /// <summary>
        /// Setups service to service auth.
        /// </summary>
        public void SetupCrossAuth()
        {
            var tokenResponse =
                new
                {
                    access_token = "token",
                };

            WireMockServer
                .Given(Request.Create()
                    .WithPath("/connect/token")
                    .UsingMethod(HttpMethods.Post))
                .RespondWith(Response.Create()
                    .WithStatusCode(StatusCodes.Status200OK)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(tokenResponse));
        }
    }
}