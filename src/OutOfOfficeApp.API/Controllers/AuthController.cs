using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application;
using OutOfOfficeApp.Application.Services.Interfaces;

namespace OutOfOfficeApp.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService ) : Controller
{
    [HttpPost("login")]
    public async  Task<IActionResult> Login([FromBody] LoginRequestDto login)
    {
        try
        {
            var response = await authService.LoginAsync(login);
            return Ok(response);
        }

        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}