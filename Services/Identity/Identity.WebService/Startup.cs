using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Common.Hosting.Configuration;
using Common.Swagger;
using Identity.Domain;
using Identity.Persistence;
using Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Identity.WebService
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
            services.AddAuthentication()
                .AddIdentityServerAuthentication();

            string connectionString = Configuration.GetConnectionString("Identity");
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(connectionString));

            // TODO: add trusted origins
            //services.AddSingleton<ICorsPolicyService>(container =>
            //{
            //    var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
            //    return new DefaultCorsPolicyService(logger)
            //    {
            //        AllowAll = true,
            //    };
            //});

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    // TODO: not safe for production environment
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var config = Configuration.GetSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();
            var certificate = X509Certificate2.CreateFromPem(
                config.Certificate, config.PrivateKey);

            var migrationAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;
            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "/Identity/Account/Login";
                    options.UserInteraction.LogoutUrl = "/Identity/Account/Logout";
                })
                .AddSigningCredential(certificate)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseNpgsql(
                            connectionString,
                            sql => sql.MigrationsAssembly(migrationAssembly));
                    };
                })
                .AddOperationalStore(
                    options => options.ConfigureDbContext = builder =>
                        builder.UseNpgsql(
                            connectionString,
                            sql => sql.MigrationsAssembly(migrationAssembly)))
                .AddAspNetIdentity<User>();

            services.AddScoped<ClientsService>();
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddControllers();
            services.AddRazorPages();

            services.AddSwaggerGen(c =>
            {
                c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)
                    ? methodInfo.Name
                    : null);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity.WebApplication", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:5001/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"identity", "full"}
                            }
                        },
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.WebApplication v1");
                    c.OAuthClientId("swagger");
                    c.OAuthUsePkce();
                });
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            // TODO: add trusted origins
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            app.UseStaticFiles();
        }
    }
}
