using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;
using OutOfOfficeApp.CoreDomain.Entities;
using System.ComponentModel;

namespace OutOfOfficeApp.API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeePostDTO employee)
        {
            try
            {
                await _employeeService.AddEmployeeAsync(employee);
                return Created();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesAsync(pageNumber, pageSize);
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
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
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
                await _employeeService.UpdateEmployeeAsync(id, employee);
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

        [HttpPost("{id}")]
        public async Task<IActionResult> DeactivateEmployee(int id)
        {
            try
            {
                await _employeeService.DeactivateEmployeeAsync(id);
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
