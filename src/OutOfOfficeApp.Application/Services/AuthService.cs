using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;
using OutOfOfficeApp.CoreDomain.Entities;

namespace OutOfOfficeApp.Application.Services;

public class AuthService(UserManager<User> userManager, ITokenService tokenService) : IAuthService
{
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto login)
    {
        var identityUser = await userManager.FindByEmailAsync(login.Email);

        if (identityUser != null)
        {
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