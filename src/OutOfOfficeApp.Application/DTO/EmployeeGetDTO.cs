using OutOfOfficeApp.CoreDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.DTO
{
    public class EmployeeGetDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Subdivision Subdivision { get; set; }
        public Position Position { get; set; }
        public EmployeeStatus Status { get; set; }
        public string PeoplePartnerName { get; set; }
        public int OutOfOfficeBalance { get; set; }
    }
}
