using Common.Hosting;
using Common.MongoDb;
using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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

            services.AddMediatR(typeof(UsersHandler));

            services.AddSingleton<IFirebaseMessaging, FirebaseMessagingService>(
                provider => new FirebaseMessagingService(FirebaseMessaging.DefaultInstance));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            ServicesExtensions.ConfigureBase(app, env, provider);
        }
    }
}
