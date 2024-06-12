using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int SubdivisionId { get; set; }
        public int PositionId { get; set; }
        public int StatusId { get; set; }
        public int PeoplePartnerId { get; set; }
        public int UserId { get; set; }
        public int OutOfOfficeBalance { get; set; }
        public byte[]? Photo { get; set; }

        public Option Subdivision { get; set; }
        public Option Position { get; set; }
        public Option Status { get; set; }
        public Employee PeoplePartner { get; set; }
        public User User { get; set; }

    }
}
