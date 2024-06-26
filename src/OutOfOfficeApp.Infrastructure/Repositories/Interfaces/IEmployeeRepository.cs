using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using OutOfOfficeApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.Repositories.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task <Employee?> GetEmployeeWithDetailsAsync(int id);
        Task<int?> GetHRManagerIdWithLeastActiveRequestsAsync();
        Task<IEnumerable<Employee>?> GetEmployeesByPositionAsync(Position position);
        Task<PagedResponse<Employee>?> GetPagedEmployeesWithDetailsAsync(int pageNumber, int pageSize);
    }
}
