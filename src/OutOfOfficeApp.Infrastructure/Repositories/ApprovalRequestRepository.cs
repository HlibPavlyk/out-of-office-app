using Microsoft.EntityFrameworkCore;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.Infrastructure.DTO;
using OutOfOfficeApp.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutOfOfficeApp.CoreDomain.Enums;

namespace OutOfOfficeApp.Infrastructure.Repositories
{
    public class ApprovalRequestRepository : GenericRepository<ApprovalRequest>, IApprovalRequestRepository
    {
        public ApprovalRequestRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ApprovalRequest?> GetApprovalRequestByLeaveRequestIdAsync(int id)
        {
            return await _context.ApprovalRequests
                .FirstOrDefaultAsync(ar => ar.LeaveRequestId == id);
        }

        public async Task<ApprovalRequest?> GetApprovalRequestWithDetailsAsync(int id)
        {
            return await _context.ApprovalRequests
                .Include(ar => ar.Approver)
                .Include(ar => ar.LeaveRequest)
                    .ThenInclude(e => e.Employee)
                        .ThenInclude(p => p.Project)
                .FirstOrDefaultAsync(ar => ar.Id == id);
        }

        public async Task<PagedResponse<ApprovalRequest>?> GetPagedApprovalRequestsWithDetailsAsync(string? userRole,
            int personId, int pageNumber, int pageSize)
        {
            var query = _context.ApprovalRequests
                .Include(ar => ar.Approver)
                .Include(ar => ar.LeaveRequest)
                    .ThenInclude(e => e.Employee)
                        .ThenInclude(p => p.Project)
                .OrderBy(ar => ar.Status == ApprovalRequestStatus.New)
                .AsQueryable();
                
            if (userRole == Position.HRManager.ToString())
            {
                query = query.Where(ar => ar.ApproverId == personId);
            }
            else if (userRole == Position.ProjectManager.ToString())
            {
                query = query.Where(ar => ar.LeaveRequest.Employee.Project.ProjectManagerId == personId);
            }
            
            var items = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalItems = await query.CountAsync();
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
