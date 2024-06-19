using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.CoreDomain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public int ProjectTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProjectManagerId { get; set; }
        public string? Comment { get; set; }
        public int StatusId { get; set; }

        public Option ProjectType { get; set; }
        public Employee ProjectManager { get; set; }
        public Option Status { get; set; }
    }
}
