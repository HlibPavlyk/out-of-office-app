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
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(ApplicationDbContext context) : base(context) { }
        public async Task<LeaveRequest?> GetLeaveRequestWithDetailsAsync(int id)
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .FirstOrDefaultAsync(lr => lr.Id == id);
        }

        public async Task<PagedResponse<LeaveRequest>?> GetPagedLeaveRequestsWithDetailsAsync(int pageNumber, int pageSize)
        {
            var items = await _context.LeaveRequests
               .Include(lr => lr.Employee)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            var totalItems = await _context.LeaveRequests.CountAsync();
            if (items == null || totalItems == 0)
            {
                return null;
            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return new PagedResponse<LeaveRequest>
            {
                Items = items,
                TotalPages = totalPages
            };
        }
    }
}
