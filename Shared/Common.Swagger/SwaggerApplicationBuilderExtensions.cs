using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Common.Swagger
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> swagger extensions.
    /// </summary>
    public static class SwaggerApplicationBuilderExtensions
    {
        private const string Prefix = "swagger";
        private const string FileName = "swagger.json";

        public static IApplicationBuilder UseSwagger(
            this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = $"{Prefix}/{{documentName}}/{FileName}";
            });
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = Prefix;
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    string url = $"/{Prefix}/{description.GroupName}/{FileName}";
                    options.SwaggerEndpoint(url, description.GroupName.ToUpperInvariant());
                }
            });

            return app;
        }
    }
}