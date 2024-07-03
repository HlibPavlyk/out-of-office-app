using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;

namespace OutOfOfficeApp.API.Controllers
{
    [ApiController]
    [Route("api/leave-requests")]
    [Authorize]
    public class LeaveRequestController(ILeaveRequestService leaveRequestService) : Controller
    {
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestPostDTO leaveRequest)
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Email)?.Value;
                await leaveRequestService.AddLeaveRequestAsync(currentUser, leaveRequest);
                return Created();
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

        [HttpGet]
        [Authorize(Roles = "Administrator, ProjectManager, Employee, HRManager")]
        public async Task<IActionResult> GetLeaveRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Email)?.Value;
                var requests = await leaveRequestService.GetLeaveRequestsAsync(currentUser, pageNumber, pageSize);
                return Ok(requests);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, ProjectManager, Employee, HRManager")]
        public async Task<IActionResult> GetLeaveRequest(int id)
        {
            try
            {
                var employee = await leaveRequestService.GetLeaveRequestByIdAsync(id);
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
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<IActionResult> UpdateLeaveRequest(int id, [FromBody] LeaveRequestPostDTO leaveRequest)
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Email)?.Value;
                await leaveRequestService.UpdateLeaveRequestAsync(id, currentUser, leaveRequest);
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

        [HttpPost("{id}/submit")]
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<IActionResult> SubmitLeaveRequest(int id)
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Email)?.Value;
                await leaveRequestService.SubmitLeaveRequestAsync(currentUser, id);
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

        [HttpPost("{id}/cancel")]
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<IActionResult> CancelLeaveRequest(int id)
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Email)?.Value;
                await leaveRequestService.CancelLeaveRequestAsync(currentUser, id);
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
