using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.DTO
{
    public class ApprovalRequestPostDTO
    {
        public int LeaveRequestId { get; set; }
        public string? Comment { get; set; }
    }
}
