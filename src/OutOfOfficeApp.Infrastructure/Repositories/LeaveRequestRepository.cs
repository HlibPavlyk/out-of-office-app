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
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(ApplicationDbContext context) : base(context) { }
        public async Task<LeaveRequest?> GetLeaveRequestWithDetailsAsync(int id)
        {
            return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .FirstOrDefaultAsync(lr => lr.Id == id);
        }

        public async Task<PagedResponse<LeaveRequest>?> GetPagedLeaveRequestsWithDetailsAsync(string? userRole,
            int personId, int pageNumber, int pageSize)
        {
            var query = _context.LeaveRequests
               .Include(lr => lr.Employee)
               .AsQueryable();

            if (userRole != Position.Administrator.ToString())
            {
                query = query.Where(lr => lr.Status != LeaveRequestStatus.Canceled);
            }
            else if (userRole == Position.HRManager.ToString())
            {
                query = query.Where(lr => lr.Employee.PeoplePartnerId == personId);
            }
            else if (userRole == Position.ProjectManager.ToString())
            {
                query = query.Where(lr => lr.Employee.Project.ProjectManagerId == personId);
            }
            else if (userRole == Position.Employee.ToString())
            {
                query = query.Where(lr => lr.EmployeeId == personId);
            }
            
            var items = await query
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            var totalItems = await query.CountAsync();
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
