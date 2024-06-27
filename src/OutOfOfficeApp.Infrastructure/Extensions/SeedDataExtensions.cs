using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OutOfOfficeApp.CoreDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.Extensions
{
    public static class SeedDataExtensions
    {
        public static async Task SeedDataAsync(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var seeder = new DataSeeder(userManager, roleManager);
                await seeder.SeedAsync();
            }
        }
    }
}
