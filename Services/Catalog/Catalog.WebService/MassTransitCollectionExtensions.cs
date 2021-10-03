using System;
using Catalog.Indexing;
using Common.Configuration;
using CSharpFunctionalExtensions;
using Domain;
using GreenPipes;
using MassTransit;
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
    }
}