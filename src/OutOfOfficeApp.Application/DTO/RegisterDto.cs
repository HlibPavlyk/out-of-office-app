using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OutOfOfficeApp.CoreDomain.Enums;

namespace OutOfOfficeApp.Application.DTO;

public class RegisterDto
{
    public string FullName { get; set; }
    public Position Position { get; set; }
    public int EmployeeId { get; set; }
}