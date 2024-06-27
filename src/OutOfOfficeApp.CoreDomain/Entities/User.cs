
using Microsoft.AspNetCore.Identity;

namespace OutOfOfficeApp.CoreDomain.Entities
{
    public class User : IdentityUser
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
