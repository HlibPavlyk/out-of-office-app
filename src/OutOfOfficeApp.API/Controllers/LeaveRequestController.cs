using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;

namespace OutOfOfficeApp.API.Controllers
{
    [ApiController]
    [Route("api/leave-request")]
    public class LeaveRequestController(ILeaveRequestService leaveRequestService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestPostDTO leaveRequest)
        {
            try
            {
                await leaveRequestService.AddLeaveRequestAsync(leaveRequest);
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
        public async Task<IActionResult> GetLeaveRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var requests = await leaveRequestService.GetLeaveRequestsAsync(pageNumber, pageSize);
                return Ok(requests);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
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
        public async Task<IActionResult> UpdateLeaveRequest(int id, [FromBody] LeaveRequestPostDTO leaveRequest)
        {
            try
            {
                await leaveRequestService.UpdateLeaveRequestAsync(id, leaveRequest);
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
        public async Task<IActionResult> SubmitLeaveRequest(int id)
        {
            try
            {
                await leaveRequestService.SubmitLeaveRequestAsync(id);
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
        public async Task<IActionResult> CancelLeaveRequest(int id)
        {
            try
            {
                await leaveRequestService.CancelLeaveRequestAsync(id);
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
