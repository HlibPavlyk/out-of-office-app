﻿using Azure;
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

namespace OutOfOfficeApp.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public EmployeeService(IUnitOfWork unitOfWork,IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
        
        public async Task<PagedResponse<EmployeeGetDTO>?> GetEmployeesAsync(int pageNumber, int pageSize)
        {
            var employees = await _unitOfWork.Employees.GetPagedEmployeesWithDetailsAsync(pageNumber, pageSize);
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
                OutOfOfficeBalance = e.OutOfOfficeBalance
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
            if(partner is not { Position: Position.HRManager })
            {
                throw new ArgumentNullException("People partner not found or is not HR Manager");
            }
            
            var newEmployee = new Employee
            {
                FullName = employee.FullName,
                Subdivision = employee.Subdivision,
                Position = employee.Position,
                Status = employee.Status,
                PeoplePartnerId = employee.PeoplePartnerId,
                OutOfOfficeBalance = employee.OutOfOfficeBalance
            };
            
            var userEmployee = new RegisterDto
            {
                FullName = employee.FullName,
                Position = employee.Position,
                Employee = newEmployee
            }; 
            
            await _authService.CreateUserByEmployee(userEmployee);
        }

        public async Task UpdateEmployeeAsync(int id, EmployeePostDTO employee)
        {
            var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (existingEmployee == null)
            {
                throw new ArgumentNullException("Employee not found");
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
            else
            {
                employee.Status = ActiveStatus.Inactive;
                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.CompleteAsync();
            }
            
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
                OutOfOfficeBalance = employee.OutOfOfficeBalance
            };

            return employeeDTO;
        }
    }
}
