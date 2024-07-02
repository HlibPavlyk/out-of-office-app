using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;

namespace OutOfOfficeApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/employees")]
    public class EmployeesController(IEmployeeService employeeService, UserManager<User> userManager) : Controller
    {
        [HttpPost]
        [Authorize(Roles = "HRManager, Administrator")]
        public async Task<IActionResult> AddEmployee(EmployeePostDTO employee)
        {
            try
            {
                await employeeService.AddEmployeeAsync(employee);
                return Created();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "HRManager, Administrator, ProjectManager")]
        public async Task<IActionResult> GetEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
                var employees = await employeeService.GetEmployeesAsync(roleClaim, pageNumber, pageSize);
                return Ok(employees);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "HRManager, Administrator, ProjectManager")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var employee = await employeeService.GetEmployeeByIdAsync(id);
                return Ok(employee);
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "HRManager, Administrator")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeePostDTO employee)
        {
            try
            {
                await employeeService.UpdateEmployeeAsync(id, employee);
                return NoContent();
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "HRManager, Administrator")]
        public async Task<IActionResult> DeactivateEmployee(int id)
        {
            try
            {
                await employeeService.DeactivateEmployeeAsync(id);
                return NoContent();
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        
        [HttpPost("{id}/assign")]
        [Authorize(Roles = "Administrator, ProjectManager")]
        public async Task<IActionResult> AssignEmployee(int id, [FromBody] EmployeeAssignDto assignDto)
        {
            try
            {
                await employeeService.AssignEmployeeToProject(id, assignDto.ProjectId);
                return NoContent();
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        
        [HttpPost("{id}/unassign")]
        [Authorize(Roles = "Administrator, ProjectManager")]
        public async Task<IActionResult> UnassignEmployee(int id)
        {
            try
            {
                await employeeService.UnassignEmployee(id);
                return NoContent();
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

    }
}
