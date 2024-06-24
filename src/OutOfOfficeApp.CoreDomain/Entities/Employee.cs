using OutOfOfficeApp.CoreDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.CoreDomain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Subdivision Subdivision { get; set; }
        public Position Position { get; set; }
        public ActiveStatus Status { get; set; }
        public int PeoplePartnerId { get; set; }
        public int OutOfOfficeBalance { get; set; }

        public Employee PeoplePartner { get; set; }

    }
}
