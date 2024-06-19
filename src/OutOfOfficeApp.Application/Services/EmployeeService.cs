using OutOfOfficeApp.Application.Services.Interfaces;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
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

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Employee>?> GetEmployeesAsync()
        {
            return await _unitOfWork.Employees.GetAllAsync();
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateEmployeeAsync(int id, Employee employee)
        {
            var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (existingEmployee == null)
            {
                throw new Exception("Employee not found");
            }
            else
            {
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
                throw new Exception("Employee not found");
            }
            else
            {
                employee.Status = EmployeeStatus.Inactive;
                _unitOfWork.Employees.Update(employee);
                await _unitOfWork.CompleteAsync();
            }
            
        }
    }
}
