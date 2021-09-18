using System;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.Persistence.Migrations.ApplicationDb
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var factory = new ApplicationDbContextFactory();
            using var context = factory.CreateDbContext(Array.Empty<string>());

            var hasher = new PasswordHasher<User>();

            var user = new User()
            {
                Email = "catalogmanager@eshop.com",
                UserName = "catalogmanager@eshop.com",
                PasswordHash = hasher.HashPassword(null, "111111"),
                EmailConfirmed = true,
            };
            context.Users.Add(user);

            context.SaveChanges();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
