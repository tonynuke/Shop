using Catalog.Indexing;
using Common.Hosting.Configuration;
using Common.Outbox.Publisher;
using Domain;
using GreenPipes;
using MassTransit;
using MassTransit.KafkaIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.WebService
{
    public static class MassTransitCollectionExtensions
    {
        public static IServiceCollection ConfigureMassTransit(
            this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection(RabbitMqConfiguration.Key).Get<RabbitMqConfiguration>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CatalogIndexerConsumer>();

                x.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(config.HostAddress, hostConfigurator =>
                    {
                        hostConfigurator.Username(config.Username);
                        hostConfigurator.Password(config.Password);
                    });
                    configurator.Publish<DomainEventBase>(p => p.Exclude = true);
                    configurator.UseMessageRetry(retryConfigurator =>
                        retryConfigurator.Interval(config.RetryLimit, config.RetryInterval));

                    configurator.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }

        public static IServiceCollection ConfigureMassTransitViaKafka(
            this IServiceCollection services, IConfiguration configuration)
        {
            //var config = configuration.GetSection(KafkaConfiguration.Key).Get<KafkaConfiguration>();

            services.AddMassTransit(x =>
            {
                x.UsingInMemory((context, configurator) =>
                {
                    configurator.ConfigureEndpoints(context);
                });
                x.AddRider(configurator =>
                {
                    configurator.AddProducer<EventEnvelope>("topic-name");
                    configurator.AddConsumer<CatalogIndexerConsumer>();
                    configurator.UsingKafka((context, factoryConfigurator) =>
                    {
                        //factoryConfigurator.Host(config.Host);
                        factoryConfigurator.Host("localhost:29092");
                        factoryConfigurator.TopicEndpoint<EventEnvelope>("topic-name", "consumer-group-name", e =>
                        {
                            e.ConfigureConsumer<CatalogIndexerConsumer>(context);
                            e.CreateIfMissing();
                        });
                    });
                });
            });

            services.AddMassTransitHostedService();

            return services;
        }
    }
}