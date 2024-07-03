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
                foreach (var role in Enum.GetValues(typeof(Position)))
                {
                    var roleString = role.ToString();
                    await _roleManager.CreateAsync(new IdentityRole(roleString));
                }
            }

            foreach (var role in Enum.GetValues(typeof(Position)))
            {
                var roleString = role.ToString();
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleString);

                if (!usersInRole.Any())
                {
                    var employee = new Employee
                    {
                        FullName = roleString,
                        Subdivision = Subdivision.Management,
                        Position = (Position)role,
                        Status = ActiveStatus.Active,
                        PeoplePartnerId = 1,
                        OutOfOfficeBalance = 100
                    };

                    var user = new User
                    {
                        UserName = $"{roleString.ToLower()}@example.com",
                        Email = $"{roleString.ToLower()}@example.com",
                        Employee = employee
                    };

                    var result = await _userManager.CreateAsync(user, "password");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, roleString);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Failed to create user for role {roleString}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
    }

}
