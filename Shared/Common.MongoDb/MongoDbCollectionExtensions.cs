using System;
using System.Reflection;
using Common.MongoDb.Entities;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Common.MongoDb
{
    public static class MongoDbCollectionExtensions
    {
        /// <summary>
        /// Configures Mongo database.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="mongoDatabaseSettings">Database settings.</param>
        /// <returns>Services collection.</returns>
        public static IServiceCollection ConfigureMongoDb(
            this IServiceCollection services,
            IConfiguration configuration,
            MongoDatabaseSettings mongoDatabaseSettings = null)
        {
            MongoEntitiesMapsRegistrar.RegisterConventions();

            var databaseConfiguration = configuration.GetSection(DatabaseConfiguration.Key).Get<DatabaseConfiguration>();
            var settings = MongoClientSettings.FromConnectionString(databaseConfiguration.ConnectionString);

            services.AddSingleton(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<IMongoDatabase>>();
                settings.ClusterConfigurator = builder =>
                {
                    if (!databaseConfiguration.EnableQueryLog)
                    {
                        return;
                    }

                    builder.Subscribe<CommandStartedEvent>(e =>
                    {
                        logger.LogInformation(
                            "CommandName {CommandName} {Command}", e.CommandName, e.Command);
                    });
                };

                var client = new MongoClient(settings);
                return client.GetDatabase(
                    databaseConfiguration.DatabaseName, mongoDatabaseSettings);
            });

            return services;
        }

        /// <summary>
        /// Adds db context.
        /// </summary>
        /// <typeparam name="TContext">Context type.</typeparam>
        /// <param name="services">Services.</param>
        /// <param name="implementationFactory">Implementation factory.</param>
        public static void AddMongoDbContext<TContext>(
            this IServiceCollection services,
            Func<IServiceProvider, TContext> implementationFactory)
            where TContext : DbContext
        {
            services.AddSingleton(implementationFactory);
        }

        /// <summary>
        /// Adds db context.
        /// </summary>
        /// <typeparam name="TContext">Context type.</typeparam>
        /// <param name="services">Services.</param>
        public static void AddMongoDbContext<TContext>(
            this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddSingleton(provider =>
            {
                var database = provider.GetRequiredService<IMongoDatabase>();
                var context = Activator.CreateInstance(typeof(TContext), database);
                return (TContext)context;
            });
        }
    }
}