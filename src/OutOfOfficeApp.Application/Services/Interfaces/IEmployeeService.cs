using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.CoreDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeGetDTO>?> GetEmployeesAsync(int page);
        Task AddEmployeeAsync(EmployeePostDTO employee);
        Task UpdateEmployeeAsync(int id, EmployeePostDTO employee);
        Task DeactivateEmployeeAsync(int id);
        Task<int> GetAmountOfEmployeesPagesAsync();
    }

}
