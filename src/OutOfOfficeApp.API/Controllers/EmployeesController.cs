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
        public async Task<IActionResult> GetEmployees(int page)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesAsync(page);
                return Ok(employees);
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

        [HttpDelete("{id}")]
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

        [HttpGet("pages")]
        public async Task<IActionResult> GetAmountOfEmployeePagesAsync()
        {
            try
            {
                var pageCount = await _employeeService.GetAmountOfEmployeesPagesAsync();
                return Ok(pageCount);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
