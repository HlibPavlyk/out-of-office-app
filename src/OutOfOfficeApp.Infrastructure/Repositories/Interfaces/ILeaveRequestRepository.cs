using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.Repositories.Interfaces
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        Task<LeaveRequest?> GetLeaveRequestWithDetailsAsync(int id);
        Task<PagedResponse<LeaveRequest>?> GetPagedLeaveRequestsWithDetailsAsync(string? userRole, int personId,
            int pageNumber, int pageSize);
    }
}
