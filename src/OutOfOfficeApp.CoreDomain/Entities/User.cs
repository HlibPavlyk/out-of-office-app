using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.CoreDomain.Entities
{
    public class User
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
