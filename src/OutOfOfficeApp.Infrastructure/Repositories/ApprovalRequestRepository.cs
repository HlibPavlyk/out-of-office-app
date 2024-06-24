using Microsoft.EntityFrameworkCore;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.Infrastructure.DTO;
using OutOfOfficeApp.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.Repositories
{
    public class ApprovalRequestRepository : GenericRepository<ApprovalRequest>, IApprovalRequestRepository
    {
        public ApprovalRequestRepository(ApplicationDbContext context) : base(context) { }
        public async Task<ApprovalRequest?> GetApprovalRequestWithDetailsAsync(int id)
        {
            return await _context.ApprovalRequests
                .Include(ar => ar.LeaveRequest)
                .Include(ar => ar.Approver)
                .FirstOrDefaultAsync(ar => ar.Id == id);
        }

        public async Task<PagedResponse<ApprovalRequest>?> GetPagedApprovalRequestsWithDetailsAsync(int pageNumber, int pageSize)
        {
            var items = await _context.ApprovalRequests
                .Include(ar => ar.LeaveRequest)
                .Include(ar => ar.Approver)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalItems = await _context.ApprovalRequests.CountAsync();
            if (items == null || totalItems == 0)
            {
                return null;
            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return new PagedResponse<ApprovalRequest>
            {
                Items = items,
                TotalPages = totalPages
            };
        }
    }
}
