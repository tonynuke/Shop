﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using Common.AspNetCore.Logging;
using Common.AspNetCore.Swagger;
using Common.Configuration;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Nest;

namespace Common.AspNetCore.Configuration
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
            // https://stackoverflow.com/questions/62475109/asp-net-core-jwt-authentication-changes-claims-sub
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddHeaderPropagation(o =>
            {
                // propagates headers if present.
                o.Headers.Add(Headers.CorrelationId);
                o.Headers.Add(HeaderNames.Authorization);
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var authConfiguration = configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();
                    var bytes = Encoding.ASCII.GetBytes(authConfiguration.Certificate);
                    var certificate = new X509Certificate2(bytes);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new X509SecurityKey(certificate),

                        // enable validation
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false
                    };
                });
            services.AddAuthorization();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddProblemDetails(ProblemDetails.ConfigureProblemDetails);
            services.AddControllers()
                .AddProblemDetailsConventions()
                .AddJsonOptions(options =>
                {
                    var converter = new JsonStringEnumConverter();
                    options.JsonSerializerOptions.Converters.Add(converter);
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

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

            services.AddHealthChecks();
            ConfigureSwaggerGen(services);

            return services;
        }

        /// <summary>
        /// Configures base application.
        /// </summary>
        /// <param name="app">Application.</param>
        /// <param name="env">Environment.</param>
        /// <param name="provider">Provider.</param>
        public static void ConfigureBase(
            IApplicationBuilder app,
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapHealthChecks("/health");
            });
        }

        private static void ConfigureSwaggerGen(IServiceCollection services)
        {
            SwaggerCollectionExtensions.ConfigureSwaggerGen(services, options =>
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