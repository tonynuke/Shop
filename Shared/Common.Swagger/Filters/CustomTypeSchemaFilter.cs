using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Swagger.Filters
{
    /// <summary>
    /// Add schema to swagger specification.
    /// </summary>
    /// <typeparam name="TSchemaType">Schema type.</typeparam>
    public class CustomTypeSchemaFilter<TSchemaType> : ISchemaFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            context.SchemaGenerator.GenerateSchema(typeof(TSchemaType), context.SchemaRepository);
        }
    }
}
