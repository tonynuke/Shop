using Common;
using Common.Database;
using FirebaseAdmin.Messaging;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Notifications.Persistence;
using Notifications.Services.Firebase;
using Notifications.Services.Users;

namespace Notifications.WebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureServicesBase(Configuration);
            services.ConfigureMongoDb(Configuration);
            services.AddMongoDbContext<NotificationsContext>();

            ConfigureMassTransit(services);
            services.AddMediatR(typeof(UsersHandler));

            services.AddSingleton<IFirebaseMessaging, FirebaseMessagingService>(
                provider => new FirebaseMessagingService(FirebaseMessaging.DefaultInstance));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            ServicesExtensions.ConfigureBase(app, env, provider);
        }

        private static void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

                x.AddRider(rider =>
                {
                    //rider.AddProducer<IUserMessage>("topic-name");
                    //rider.AddConsumer<NotificationsConsumer>();
                    //rider.UsingKafka((context, k) =>
                    //{
                    //    k.Host("localhost:29092");
                    //    k.TopicEndpoint<IUserMessage>(
                    //        "topic-name",
                    //        "consumer-group-name",
                    //        e =>
                    //        {
                    //            e.ConfigureConsumer<NotificationsConsumer>(context);
                    //            e.CreateIfMissing(options => { });
                    //        });
                    //});
                });
            });
            services.AddMassTransitHostedService();
        }
    }
}
