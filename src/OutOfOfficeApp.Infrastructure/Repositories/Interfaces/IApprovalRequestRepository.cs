﻿using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.Repositories.Interfaces
{
    public interface IApprovalRequestRepository : IGenericRepository<ApprovalRequest>
    {
        Task<ApprovalRequest?> GetApprovalRequestWithDetailsAsync(int id);
        Task<PagedResponse<ApprovalRequest>?> GetPagedApprovalRequestsWithDetailsAsync(int pageNumber, int pageSize);
    }
}
