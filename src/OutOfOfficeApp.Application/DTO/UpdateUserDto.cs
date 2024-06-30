using OutOfOfficeApp.CoreDomain.Enums;

namespace OutOfOfficeApp.Application.DTO;

public class UpdateUserDto
{
    public string PreviousFullName { get; set; }
    public string FullName { get; set; }
    public Position Position { get; set; }
}