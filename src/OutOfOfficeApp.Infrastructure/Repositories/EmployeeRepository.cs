using Microsoft.EntityFrameworkCore;
using OutOfOfficeApp.CoreDomain.Entities;
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
        private readonly int pageSize = 2;
        public EmployeeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Employee>?> GetAllEmployeesWithDetailsAsync()
        {
            return await _context.Set<Employee>()
                .Include(e => e.PeoplePartner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>?> GetAllEmployeesWithDetailsByPageAsync(int page)
        {
            return await _context.Set<Employee>()
                .Include(e => e.PeoplePartner)
                .Where(e => e.Id > (page - 1) * pageSize && e.Id <= page * pageSize)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
        {
            return await _context.Set<Employee>()
                .Include(e => e.PeoplePartner)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
