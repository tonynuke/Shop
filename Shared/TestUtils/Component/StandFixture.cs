using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace TestUtils.Component
{
    /// <summary>
    /// Stand.
    /// </summary>
    /// <typeparam name="TStartup">Startup.</typeparam>
    public class StandFixture<TStartup> : IDisposable
        where TStartup : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandFixture{TStartup}"/> class.
        /// </summary>
        public StandFixture()
        {
            int port = WireMockServer.Ports.Single();
            Host = new WebApplicationFixture<TStartup>(port);
        }

        /// <summary>
        /// Gets web application.
        /// </summary>
        public WebApplicationFixture<TStartup> Host { get; }

        /// <summary>
        /// Gets wireMock server.
        /// </summary>
        public WireMockServer WireMockServer { get; } = WireMockServer.Start();

        /// <inheritdoc/>
        public void Dispose()
        {
            WireMockServer.Stop();
            WireMockServer.Dispose();

            Host.Dispose();
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