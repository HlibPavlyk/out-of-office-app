using OutOfOfficeApp.CoreDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.Repositories.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<Employee?> GetEmployeeWithDetailsAsync(int id);
        Task<IEnumerable<Employee>?> GetAllEmployeesWithDetailsAsync();
    }
}
