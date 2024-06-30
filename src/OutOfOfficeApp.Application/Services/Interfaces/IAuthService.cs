using Microsoft.AspNetCore.Identity.Data;
using OutOfOfficeApp.Application.DTO;

namespace OutOfOfficeApp.Application.Services.Interfaces;

public interface IAuthService
{
    Task CreateUserByEmployee(RegisterDto registerDto);
    Task UpdateUserByEmployee(UpdateUserDto updateUserDto);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto login);
}
