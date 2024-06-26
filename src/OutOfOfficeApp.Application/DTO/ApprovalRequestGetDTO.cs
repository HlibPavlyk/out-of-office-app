using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.DTO
{
    public class ApprovalRequestGetDTO
    {
        public int Id { get; set; }
        public EmployeeNameDTO Approver { get; set; }
        public LeaveRequestGetDTO LeaveRequest { get; set; }
        public ApprovalRequestStatus Status { get; set; }
        public string? Comment { get; set; }
    }
}
