using System;
using System.IO;
using System.Reflection;
using Common.Swagger;
using Common.Swagger.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.AspNetCore.Swagger
{
    /// <summary>
    /// <see cref="IServiceCollection"/> swagger extensions.
    /// </summary>
    public static class SwaggerCollectionExtensions
    {
        public static IServiceCollection ConfigureSwaggerGen(
            this IServiceCollection services,
            Action<SwaggerGenOptions> setupAction = null)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                setupAction?.Invoke(options);

                options.OperationFilter<SwaggerDefaultValues>();
                options.OperationFilter<ResponseOperationFilter<ProblemDetails>>(StatusCodes.Status400BadRequest);
                options.OperationFilter<ResponseOperationFilter<ProblemDetails>>(StatusCodes.Status409Conflict);
                options.SchemaFilter<CustomTypeSchemaFilter<ProblemDetails>>();

                options.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)
                    ? methodInfo.Name
                    : null);

                options.MapType<decimal>(() => new OpenApiSchema
                {
                    Type = "number",
                    Format = "decimal"
                });

                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string executionPath = Path.GetDirectoryName(assemblyLocation);
                string[] documentationPaths = Directory.GetFiles(executionPath, "*.xml");
                foreach (var documentationPath in documentationPaths)
                {
                    options.IncludeXmlComments(documentationPath);
                }
            });

            return services;
        }
    }
}