using System.Text.Json.Serialization;
using Common.ApiErrors;
using Common.Hosting.Auth;
using Common.Logging;
using Common.Swagger;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Prometheus;

namespace Common.Hosting
{
    /// <summary>
    /// Services configuration extensions.
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Configures base services dependencies.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureServicesBase(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddHeaderPropagation(o =>
            {
                // propagates headers if present.
                o.Headers.Add(Headers.CorrelationId);
                o.Headers.Add(HeaderNames.Authorization);
            });

            services.ConfigureAuthentication(configuration);
            services.ConfigureAuthorization();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddProblemDetails(Common.ApiErrors.ProblemDetailsExtensions.ConfigureProblemDetails);
            services.AddControllers()
                .AddProblemDetailsConventions()
                .AddJsonOptions(options =>
                {
                    var converter = new JsonStringEnumConverter();
                    options.JsonSerializerOptions.Converters.Add(converter);
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            services.AddHealthChecks().ForwardToPrometheus();
            services.ConfigureApiVersioning();
            services.ConfigureSwaggerGen();

            return services;
        }

        /// <summary>
        /// Configures base application.
        /// </summary>
        /// <param name="app">Application.</param>
        /// <param name="env">Environment.</param>
        /// <param name="provider">Provider.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IApplicationBuilder ConfigureBase(
            this IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            app.UseProblemDetails();
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Overrides problem details generation.
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseSwagger(provider);

            // TODO: check HttpsRedirection
            // app.UseHttpsRedirection();
            app.UseHeaderPropagation();

            app.UseRouting();

            // TODO: add trusted origins
            app.UseCors(builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapHealthChecks("/health");
                endpoints.MapMetrics();
            });

            return app;
        }

        public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
        {
            return SwaggerCollectionExtensions.ConfigureSwaggerGen(services, options =>
            {
                options.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description = "Insert JWT with Bearer into field",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        Type = SecuritySchemeType.Http
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            new string[] { }
                        }
                    });
            });
        }
    }
}