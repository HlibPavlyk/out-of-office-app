using Microsoft.AspNetCore.Identity;
using OutOfOfficeApp.CoreDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutOfOfficeApp.CoreDomain.Enums;

namespace OutOfOfficeApp.Infrastructure
{
    public class DataSeeder
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Employee"));
                await _roleManager.CreateAsync(new IdentityRole("HRManager"));
                await _roleManager.CreateAsync(new IdentityRole("ProjectManager"));
                await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            var adminRole = await _roleManager.FindByNameAsync("Administrator");
            if(adminRole?.Name == null)
            {
                throw new InvalidOperationException("No admin role found");
            }

            var admins = await _userManager.GetUsersInRoleAsync(adminRole.Name);

            if (!admins.Any())
            {
                var adminEmployee = new Employee
                {
                    FullName = "Admin",
                    Subdivision = Subdivision.Management,
                    Position = Position.Administrator,
                    Status = ActiveStatus.Active,
                    PeoplePartnerId = 1,
                    OutOfOfficeBalance = 100
                };
                
                var adminUser = new User { UserName = "admin@example.com", Email = "admin@example.com",
                    Employee = adminEmployee};
                var result = await _userManager.CreateAsync(adminUser, "password");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
        }
    }

}
