using Basket.Persistence;
using Basket.Services;
using Common;
using Common.Database;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.WebService
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
            services.AddMongoDbContext<BasketContext>();
            services.AddSingleton<BasketsService>();
            //var catalogEndpointUrl = Configuration.GetConnectionString("Catalog");

            //var crossAuthConfig = Configuration.GetSection(CrossServiceAuthConfiguration.Key);
            //services.Configure<CrossServiceAuthConfiguration>(crossAuthConfig);
            //services.AddScoped<CrossServiceAuthHandler>();
            //services.ConfigureCrossServiceAuthHttpClient();

            //services
            //    .ConfigureHttpClient<ICatalogClient, CatalogClient>(catalogEndpointUrl)
            //    .AddHttpMessageHandler<CrossServiceAuthHandler>();

            TypeAdapterConfig.GlobalSettings.Scan(typeof(Mapper).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            ServicesExtensions.ConfigureBase(app, env, provider);
            MigrationsExtensions.ApplyDevelopmentMigrations<BasketContext>(app, env);
        }
    }
}
