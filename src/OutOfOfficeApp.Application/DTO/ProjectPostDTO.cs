using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.DTO
{
    public class ProjectPostDTO
    {
        public ProjectType ProjectType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int ProjectManagerId { get; set; }
        public string? Comment { get; set; }
        public ActiveStatus Status { get; set; }
    }
}
