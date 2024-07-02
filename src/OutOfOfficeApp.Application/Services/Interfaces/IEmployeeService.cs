using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<PagedResponse<EmployeeGetDTO>?> GetEmployeesAsync(int pageNumber, int pageSize);
        Task<EmployeeGetDTO> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(EmployeePostDTO employee);
        Task UpdateEmployeeAsync(int id, EmployeePostDTO employee);
        Task DeactivateEmployeeAsync(int id);
        Task AssignEmployeeToProject(int id, int projectId);
        Task UnassignEmployee(int id);
    }

}
