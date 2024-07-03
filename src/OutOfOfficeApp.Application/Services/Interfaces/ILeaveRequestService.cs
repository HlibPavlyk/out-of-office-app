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
        Task<PagedResponse<LeaveRequestGetDTO>?> GetLeaveRequestsAsync(string? userEmail, int pageNumber, int pageSize);
        Task<LeaveRequestGetDTO> GetLeaveRequestByIdAsync(int id);
        Task AddLeaveRequestAsync(string? userEmail, LeaveRequestPostDTO request);
        Task UpdateLeaveRequestAsync(int id, string? userEmail, LeaveRequestPostDTO request);
        Task CancelLeaveRequestAsync(string? userEmail, int id);
        Task SubmitLeaveRequestAsync(string? userEmail, int id);
    }
}
