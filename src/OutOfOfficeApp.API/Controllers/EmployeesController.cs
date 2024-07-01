using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;

namespace OutOfOfficeApp.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController(IEmployeeService employeeService) : Controller
    {
        [HttpPost]
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
        [Authorize]
        public async Task<IActionResult> GetEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var employees = await employeeService.GetEmployeesAsync(pageNumber, pageSize);
                return Ok(employees);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
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

    }
}
