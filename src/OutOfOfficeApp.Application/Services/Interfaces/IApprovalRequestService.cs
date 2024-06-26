using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.Services.Interfaces
{
    public interface IApprovalRequestService
    {
        Task<PagedResponse<ApprovalRequestGetDTO>?> GetApprovalRequestsAsync(int pageNumber, int pageSize);
        Task<ApprovalRequestGetDTO> GetApprovalRequestByIdAsync(int id);
        Task AddApprovalRequestAsync(int leaveRequestId);
        Task RejectApprovalRequestAsync(int id, int issuerId);
        Task ApproveApprovalRequestAsync(int id, int issuerId);
    }
}
