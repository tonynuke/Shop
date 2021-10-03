using System;
using Catalog.Brands;
using Catalog.Indexing;
using Catalog.Items;
using Catalog.Messages.Events;
using Catalog.Persistence;
using Common;
using Common.Configuration;
using Common.Database;
using Common.Outbox;
using Common.Scheduler;
using DataAccess.Entities;
using Domain;
using Hangfire;
using Mapster;
using MassTransit;
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
            MongoEntitiesMapsRegistrar.RegisterHierarchy(typeof(DomainEventBase), typeof(ItemAdded).Assembly);
            services.AddMongoDbContext<CatalogContext>();
            services.AddSingleton<CatalogIndexer>();
            services.AddSingleton<BrandsService>();
            services.AddSingleton<CatalogItemsService>();
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Mapper).Assembly);

            services.ConfigureMassTransit(Configuration);
            services.ConfigureHangfire(Configuration);

            services.AddScoped(provider =>
            {
                var context = provider.GetRequiredService<CatalogContext>();
                var publishEndpoint = provider.GetRequiredService<IPublishEndpoint>();
                return new OutboxRabbitMqPublisher(context, publishEndpoint);
            });
        }

        private static bool Handler(AuthorizationHandlerContext arg)
        {
            return arg.User.HasClaim(AuthorizationPolicies.Catalog, "all");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            ServicesExtensions.ConfigureBase(app, env, provider);
            MigrationsExtensions.ApplyDevelopmentMigrations<CatalogContext>(app, env);
            app.UseHangfireDashboard();
            ConfigureJobs();
        }

        private static void ConfigureJobs()
        {
            RecurringJob.AddOrUpdate(
                (OutboxRabbitMqPublisher service) => service.Publish(), Cron.Minutely);
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
