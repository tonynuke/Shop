using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestUtils.Component
{
    /// <summary>
    /// Web application factory fixture.
    /// </summary>
    /// <typeparam name="TStartup">Startup.</typeparam>
    public class WebApplicationFixture<TStartup>
        : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApplicationFixture{TStartup}"/> class.
        /// </summary>
        /// <param name="networkDependencyPort">Network dependencies endpoint port.</param>
        public WebApplicationFixture(int networkDependencyPort)
        {
            using var stream = AppSettingsHelper.GetOverridenAppSettings(networkDependencyPort);
            _configuration = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Gets configuration.
        /// </summary>
        public IConfiguration Configuration => _configuration;

        /// <inheritdoc/>
        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder
                .UseContentRoot(Directory.GetCurrentDirectory()));
        }

        /// <inheritdoc/>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder
                .ConfigureAppConfiguration(
                    configurationBuilder =>
                    {
                        configurationBuilder.Sources.Clear();
                        configurationBuilder.AddConfiguration(_configuration);
                    })
                .ConfigureLogging(loggingBuilder =>
                {
                    // configure logging
                    // loggingBuilder.ClearProviders();
                }));
        }
    }
}