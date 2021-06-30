using System;
using Identity.Persistence;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.WebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var env = serviceScope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                if (env.IsDevelopment())
                {
                    Migrate<PersistedGrantDbContext>(serviceScope.ServiceProvider);
                    Migrate<ConfigurationDbContext>(serviceScope.ServiceProvider);
                    Migrate<ApplicationDbContext>(serviceScope.ServiceProvider);
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void Migrate<TContext>(IServiceProvider serviceProvider)
            where TContext : DbContext
        {
            var context = serviceProvider.GetRequiredService<TContext>();
            context.Database.Migrate();
        }
    }
}
