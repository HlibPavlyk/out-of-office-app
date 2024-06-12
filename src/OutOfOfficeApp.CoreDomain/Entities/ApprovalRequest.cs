using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.Entities
{
    public class ApprovalRequest
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public int LeaveRequestId { get; set; }
        public int StatusId { get; set; }
        public string? Comment { get; set; }

        public Employee Approver { get; set; }
        public LeaveRequest LeaveRequest { get; set; }
        public Option Status { get; set; }
    }
}
