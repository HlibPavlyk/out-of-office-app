using Microsoft.AspNetCore.Identity.Data;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.CoreDomain.Entities;

namespace OutOfOfficeApp.Application.Services.Interfaces;

public interface IAuthService
{
    Task CreateUserByEmployee(RegisterDto registerDto);
    Task UpdateUserByEmployee(UpdateUserDto updateUserDto);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto login);
}
