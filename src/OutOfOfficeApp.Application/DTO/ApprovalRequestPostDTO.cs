using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.DTO
{
    public class ApprovalRequestPostDTO
    {
        public int? IssuerId { get; set; }
        public string? Comment { get; set; }
    }
}
