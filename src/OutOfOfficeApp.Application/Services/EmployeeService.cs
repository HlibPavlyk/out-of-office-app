using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using OutOfOfficeApp.Infrastructure.DTO;
using OutOfOfficeApp.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace OutOfOfficeApp.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;

        public EmployeeService(IUnitOfWork unitOfWork,IAuthService authService, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _userManager = userManager;
        }
        
        public async Task<PagedResponse<EmployeeGetDTO>?> GetEmployeesAsync(string? userRole, int pageNumber, int pageSize)
        {
            if (userRole == null)
            {
                throw new ArgumentNullException("User role not found");
            }
            var isProjectManager = userRole == Position.ProjectManager.ToString();
            
            var employees = await _unitOfWork.Employees.GetPagedEmployeesWithDetailsAsync(pageNumber, pageSize, isProjectManager);
            if (employees == null)
            {
                return null;
            }

            var employeeDtos = employees.Items.Select(e => new EmployeeGetDTO
            {
                Id = e.Id,
                FullName = e.FullName,
                Subdivision = e.Subdivision,
                Position = e.Position,
                Status = e.Status,
                PeoplePartner = new EmployeeNameDTO
                {
                    Id = e.PeoplePartner.Id,
                    FullName = e.PeoplePartner.FullName
                },
                OutOfOfficeBalance = e.OutOfOfficeBalance,
                ProjectId = e.ProjectId
            }).ToList();

            var response = new PagedResponse<EmployeeGetDTO>
            {
                Items = employeeDtos,
                TotalPages = employees.TotalPages
            };

            return response;
        }

        public async Task AddEmployeeAsync(EmployeePostDTO employee)
        {
            var partner = await _unitOfWork.Employees.GetEmployeeWithDetailsAsync(employee.PeoplePartnerId);
            if(partner == null || partner.Position != Position.HRManager)
            {
                throw new ArgumentNullException("People partner not found or is not HR Manager");
            }

            /*using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {*/
                    var newEmployee = new Employee
                    {
                        FullName = employee.FullName,
                        Subdivision = employee.Subdivision,
                        Position = employee.Position,
                        Status = employee.Status,
                        PeoplePartnerId = employee.PeoplePartnerId,
                        OutOfOfficeBalance = employee.OutOfOfficeBalance
                    };
                    
                    await _unitOfWork.Employees.AddAsync(newEmployee);
                    await _unitOfWork.CompleteAsync();
                    
                    var userEmployee = new RegisterDto
                    {
                        FullName = employee.FullName,
                        Position = employee.Position,
                        EmployeeId = newEmployee.Id
                    };

                    await _authService.CreateUserByEmployee(userEmployee);
                   // await transaction.CommitAsync();
                /*}
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }*/
        }


        public async Task UpdateEmployeeAsync(int id, EmployeePostDTO employee)
        {
            var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (existingEmployee == null)
            {
                throw new ArgumentNullException("Employee not found");
            }

            if (existingEmployee.FullName == "Admin")
            {
                throw new InvalidOperationException("Admin cannot be updated");
            }
            else
            {
                var userEmployee = new UpdateUserDto
                {
                    PreviousFullName = existingEmployee.FullName,
                    FullName = employee.FullName,
                    Position = employee.Position
                };
                await _authService.UpdateUserByEmployee(userEmployee);
                
                existingEmployee.FullName = employee.FullName;
                existingEmployee.Subdivision = employee.Subdivision;
                existingEmployee.Position = employee.Position;
                existingEmployee.Status = employee.Status;
                existingEmployee.PeoplePartnerId = employee.PeoplePartnerId;
                existingEmployee.OutOfOfficeBalance = employee.OutOfOfficeBalance;

                _unitOfWork.Employees.Update(existingEmployee);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeactivateEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employee == null)
            {
                throw new ArgumentNullException("Employee not found");
            }
            
            if (employee.FullName == "Admin")
            {
                throw new InvalidOperationException("Admin cannot be updated");
            }
            else
            {
                employee.Status = ActiveStatus.Inactive;
                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.CompleteAsync();
            }
            
        }

        public async Task AssignEmployeeToProject(int id, int projectId)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null)
            {
                throw new ArgumentNullException("Employee not found");
            }
            if (employee.FullName == "Admin")
            {
                throw new InvalidOperationException("Admin cannot be updated");
            }

            var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
            if (project == null || project.Status == ActiveStatus.Inactive)
            {
                throw new ArgumentNullException("Project not found or is inactive");
            }
            
            employee.ProjectId = projectId;
            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UnassignEmployee(int id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null)
            {
                throw new ArgumentNullException("Employee not found");
            }
            if (employee.FullName == "Admin")
            {
                throw new InvalidOperationException("Admin cannot be updated");
            }

            if (employee.ProjectId == null)
            {
                throw new InvalidOperationException("Employee is already unassign");
            }
            
            employee.ProjectId = null;
            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<EmployeeGetDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.Employees.GetEmployeeWithDetailsAsync(id);
            if (employee == null)
            {
                throw new ArgumentNullException("Employee not found");
            }

            var employeeDTO = new EmployeeGetDTO
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Subdivision = employee.Subdivision,
                Position = employee.Position,
                Status = employee.Status,
                PeoplePartner = new EmployeeNameDTO
                {
                    Id = employee.PeoplePartner.Id,
                    FullName = employee.PeoplePartner.FullName
                },
                OutOfOfficeBalance = employee.OutOfOfficeBalance,
                ProjectId = employee.ProjectId
            };

            return employeeDTO;
        }
    }
}
