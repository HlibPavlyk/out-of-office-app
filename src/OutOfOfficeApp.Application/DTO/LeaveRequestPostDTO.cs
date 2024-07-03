using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.DTO
{
    public class LeaveRequestPostDTO
    {
        public AbsenceReason AbsenceReason { get; set; }

        [SwaggerSchema(Format = "yyyy-MM-dd")]
        public DateOnly StartDate { get; set; }

        [SwaggerSchema(Format = "yyyy-MM-dd")]
        public DateOnly EndDate { get; set; }
        public string? Comment { get; set; }
    }
    
}
