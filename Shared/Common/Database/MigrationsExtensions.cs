using DataAccess;
using DataAccess.Migrations.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Common.Database
{
    /// <summary>
    /// Migrations configuration extensions.
    /// </summary>
    public static class MigrationsExtensions
    {
        /// <summary>
        /// Applies migration if development environment is checked.
        /// </summary>
        /// <typeparam name="TContext">Context.</typeparam>
        /// <param name="app">Application.</param>
        /// <param name="env">Environment.</param>
        public static void ApplyDevelopmentMigrations<TContext>(
            IApplicationBuilder app, IWebHostEnvironment env)
            where TContext : DbContext
        {
            if (!env.IsDevelopment())
            {
                return;
            }

            var context = app.ApplicationServices.GetRequiredService<TContext>();
            var migrations = context.GetAllMigrations();
            var migrationsRunner = new MigrationsRunner(context);
            migrationsRunner.ApplyMigrations(migrations).Wait();
        }
    }
}