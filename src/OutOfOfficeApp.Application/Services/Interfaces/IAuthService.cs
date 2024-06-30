using Microsoft.AspNetCore.Identity.Data;
using OutOfOfficeApp.Application.DTO;

namespace OutOfOfficeApp.Application.Services.Interfaces;

public interface IAuthService
{
    public Task CreateUserByEmployee(RegisterDto registerDto);
    public Task<LoginResponseDto> LoginAsync(LoginRequestDto login);
}
