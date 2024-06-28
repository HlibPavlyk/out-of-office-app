using System.Reflection.Metadata.Ecma335;

namespace OutOfOfficeApp.Application.DTO;

public class LoginResponseDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public IEnumerable<string> Roles { get; set; }
}