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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
        {
            return await _context.Set<Employee>()
                .Include(e => e.PeoplePartner)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<PagedResponse<Employee>?> GetPagedEmployeesWithDetailsAsync(int pageNumber, int pageSize)
        {
            var items = await _context.Employees
                .Include(e => e.PeoplePartner)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalItems = await _context.Employees.CountAsync();
            if (items == null || totalItems == 0)
            {
                return null;
            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return new PagedResponse<Employee>
            {
                Items = items,
                TotalPages = totalPages
            };
        }
    }
}
