using OutOfOfficeApp.CoreDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.CoreDomain.Entities
{
    public class ApprovalRequest
    {
        public int Id { get; set; }
        public int ApproverId { get; set; }
        public int LeaveRequestId { get; set; }
        public ApprovalRequestStatus Status { get; set; }
        public string? Comment { get; set; }

        public Employee Approver { get; set; }
        public LeaveRequest LeaveRequest { get; set; }
    }
}
