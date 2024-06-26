using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services;
using OutOfOfficeApp.Application.Services.Interfaces;

namespace OutOfOfficeApp.API.Controllers
{
    [ApiController]
    [Route("api/approval-request")]
    public class ApprovalRequestController : Controller
    {
        private readonly IApprovalRequestService _approvalRequestService;

        public ApprovalRequestController(IApprovalRequestService approvalRequestService)
        {
            _approvalRequestService = approvalRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetApprovalRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var requests = await _approvalRequestService.GetApprovalRequestsAsync(pageNumber, pageSize);
                return Ok(requests);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApprovalRequest(int id)
        {
            try
            {
                var approvalRequest = await _approvalRequestService.GetApprovalRequestByIdAsync(id);
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
        public async Task<IActionResult> ApproveApprovalRequest(int id, [FromBody] ApprovalRequestPostDTO issuerData)
        {
            try
            {
                await _approvalRequestService.ApproveApprovalRequestAsync(id, issuerData);
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
        public async Task<IActionResult> RejectApprovalRequest(int id, [FromBody] ApprovalRequestPostDTO issuerData)
        {
            try
            {
                await _approvalRequestService.ApproveApprovalRequestAsync(id, issuerData);
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
