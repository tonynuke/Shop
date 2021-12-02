using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Swagger.Filters
{
    /// <summary>
    /// Add response type to the operation.
    /// </summary>
    /// <typeparam name="TResponseType">Schema type.</typeparam>
    public class ResponseOperationFilter<TResponseType> : IOperationFilter
    {
        private readonly int _statusCode;
        private readonly string _description;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseOperationFilter{TResponseType}"/> class.
        /// </summary>
        /// <param name="statusCode">Status code.</param>
        /// <param name="description">Description.</param>
        /// <remarks>
        /// Default <paramref name="description"/> is empty string for open api compatibility reasons.
        /// </remarks>
        public ResponseOperationFilter(int statusCode, string description = "")
        {
            _statusCode = statusCode;
            _description = description;
        }

        private string StatusCodeString => _statusCode.ToString();

        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey(StatusCodeString))
            {
                return;
            }

            var openApiResponse = GetOpenApiResponse();
            operation.Responses.Add(StatusCodeString, openApiResponse);
        }

        private OpenApiResponse GetOpenApiResponse()
        {
            var openApiMediaType = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Id = typeof(TResponseType).Name,
                        Type = ReferenceType.Schema
                    }
                }
            };

            return new OpenApiResponse
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    { MediaTypeNames.Application.Json, openApiMediaType }
                },
                Description = _description
            };
        }
    }
}