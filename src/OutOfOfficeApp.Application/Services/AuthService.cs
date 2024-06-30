using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using OutOfOfficeApp.Infrastructure.Repositories.Interfaces;

namespace OutOfOfficeApp.Application.Services;

public class AuthService(UserManager<User> userManager, ITokenService tokenService, IUnitOfWork unitOfWork) : IAuthService
{
    private const string defaultPassword = "password";
    private const string defaultEmailDomain = "@example.com";

    public async Task CreateUserByEmployee(RegisterDto registerDto)
    {
        var email = registerDto.FullName.ToLower() + defaultEmailDomain;
        var user = new User
        {
            UserName = email,
            Email = email,
            Employee = registerDto.Employee
        };

        var result = await userManager.CreateAsync(user, defaultPassword);

        if (result.Succeeded)
        {
            result = await userManager.AddToRoleAsync(user, registerDto.Position.ToString());
            if (result.Succeeded)
            {
                return;
            }
        }
        
        throw new AuthenticationException("User creation failed");
    }
    
    public async Task UpdateUserByEmployee(UpdateUserDto updateUserDto)
    {
        var email = updateUserDto.FullName.ToLower() + defaultEmailDomain;
        var user = await userManager.FindByEmailAsync(updateUserDto.PreviousFullName.ToLower() + defaultEmailDomain);

        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }

        user.UserName = email;
        user.Email = email;

        var currentRoles = await userManager.GetRolesAsync(user);

        var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
        {
            throw new AuthenticationException("Failed to remove user roles");
        }

        var addResult = await userManager.AddToRoleAsync(user, updateUserDto.Position.ToString());
        if (!addResult.Succeeded)
        {
            throw new AuthenticationException("Failed to add user to role");
        }

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new AuthenticationException("User update failed");
        }
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto login)
    {
        var identityUser = await userManager.FindByEmailAsync(login.Email);

        if (identityUser != null)
        {
            /*var relatedEmployee = await unitOfWork.Employees.GetByIdAsync(identityUser.EmployeeId);
            if (relatedEmployee == null || relatedEmployee.Status == ActiveStatus.Inactive)
            {
                throw new AuthenticationException("User is inactive");
            }*/

            var result = await userManager.CheckPasswordAsync(identityUser, login.Password);

            if (result)
            {
                var roles = await userManager.GetRolesAsync(identityUser);


                if (identityUser.Email != null)
                {
                    var jwtToken = tokenService.CreateToken(identityUser.Email, roles);
                
                    return new LoginResponseDto
                    {
                        Email = login.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };
                }
            }
        }
        throw new AuthenticationException("Invalid email or password");
    }
}