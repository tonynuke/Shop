using System;
using Catalog.Brands;
using Catalog.Indexing;
using Catalog.Items;
using Catalog.Persistence;
using Common.AspNetCore;
using Common.AspNetCore.Configuration;
using Common.Configuration;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using Nest;

namespace Catalog.WebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new NameSerializer());

            services.ConfigureServicesBase(Configuration);
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.Catalog, policy =>
                {
                    policy.RequireAssertion(Handler);
                });
            });

            AddElasticSearch(services, Configuration);
            services.ConfigureMongoDb(Configuration);
            services.AddMongoDbContext<CatalogContext>();
            services.AddSingleton<IndexingService>();
            services.AddSingleton<BrandsService>();
            services.AddSingleton<CatalogItemsService>();
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Mapper).Assembly);
        }

        private bool Handler(AuthorizationHandlerContext arg)
        {
            return
                arg.User.HasClaim(AuthorizationPolicies.Catalog, "all");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            ServicesExtensions.ConfigureBase(app, env, provider);
            MigrationsExtensions.ApplyDevelopmentMigrations<CatalogContext>(app, env);
        }

        private static void AddElasticSearch(IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration
                .GetSection(ElasticSearchConfiguration.Key)
                .Get<ElasticSearchConfiguration>();

            var uri = new Uri(config.Url);
            var settings = new ConnectionSettings(uri)
                .DefaultIndex(config.IndexName)
                .DefaultMappingFor<Item>(m => m);

            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);

            var createIndexResponse = client.Indices.Create(
                config.IndexName,
                index => index.Map<Item>(x => x.AutoMap()));
        }
    }
}
