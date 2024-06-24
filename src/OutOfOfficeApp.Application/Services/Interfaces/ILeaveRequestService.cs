using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.Services.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<PagedResponse<LeaveRequestGetDTO>?> GetEmployeesAsync(int pageNumber, int pageSize);
        Task<LeaveRequestGetDTO> GetEmployeeByIdAsync(int id);
        Task AddLeaveRequestAsync(LeaveRequestPostDTO employee);
        Task UpdateLeaveRequestAsync(int id, LeaveRequestPostDTO employee);
        Task CancelLeaveRequestAsync(int id);
        Task SubmitLeaveRequestAsync(int id);
    }
}
