using System;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Catalog.WebService
{
    /// <summary>
    /// Custom host for swagger generation.
    /// </summary>
    /// <remarks>
    /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#use-the-cli-tool-with-a-custom-host-configuration
    /// </remarks>
    public class SwaggerHostFactory
    {
        /// <summary>
        /// Creates the host.
        /// </summary>
        /// <returns>Host.</returns>
        public static IHost CreateHost()
        {
            return Host
                .CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<SwaggerStartup>();
                })
                .Build();
        }

        public class SwaggerStartup
        {
            /// <summary>
            /// Gets configuration.
            /// </summary>
            public IConfiguration Configuration { get; }

            public SwaggerStartup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public void ConfigureServices(IServiceCollection services)
            {
                services.ConfigureServicesBase(Configuration);
            }

            public void Configure(
                IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
            {
                ServicesExtensions.ConfigureBase(app, env, provider);
            }
        }
    }
}