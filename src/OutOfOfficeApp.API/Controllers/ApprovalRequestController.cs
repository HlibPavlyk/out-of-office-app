using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;

namespace OutOfOfficeApp.API.Controllers
{
    [ApiController]
    [Route("api/approval-requests")]
    [Authorize]
    public class ApprovalRequestController(IApprovalRequestService approvalRequestService) : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Administrator, ProjectManager, HRManager")]
        public async Task<IActionResult> GetApprovalRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Email)?.Value;
                var requests = await approvalRequestService.GetApprovalRequestsAsync(currentUser, pageNumber, pageSize);
                return Ok(requests);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, ProjectManager, HRManager")]
        public async Task<IActionResult> GetApprovalRequest(int id)
        {
            try
            {
                var approvalRequest = await approvalRequestService.GetApprovalRequestByIdAsync(id);
                return Ok(approvalRequest);
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

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Administrator, ProjectManager, HRManager")]
        public async Task<IActionResult> ApproveApprovalRequest(int id, [FromBody] ApprovalRequestPostDTO issuerData)
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Email)?.Value;
                await approvalRequestService.ApproveApprovalRequestAsync(id, currentUser, issuerData);
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

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Administrator, ProjectManager, HRManager")]
        public async Task<IActionResult> RejectApprovalRequest(int id, [FromBody] ApprovalRequestPostDTO issuerData)
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Email)?.Value;
                await approvalRequestService.RejectApprovalRequestAsync(id, currentUser, issuerData);
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
