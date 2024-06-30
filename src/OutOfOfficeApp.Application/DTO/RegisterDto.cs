using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;

namespace OutOfOfficeApp.Application.DTO;

public class RegisterDto
{
    public string FullName { get; set; }
    public Position Position { get; set; }
    public Employee Employee { get; set; }
}