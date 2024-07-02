using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;

namespace OutOfOfficeApp.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectController(IProjectService projectService) : Controller
    {
        [HttpPost]
        [Authorize(Roles = "Administrator, ProjectManager")]
        public async Task<IActionResult> AddProject(ProjectPostDTO project)
        {
            try
            {
                await projectService.AddProjectAsync(project);
                return Created();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "HRManager, Administrator, ProjectManager")]
        public async Task<IActionResult> GetProjects([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Email)?.Value;
                var projects = await projectService.GetProjectsAsync(currentUser, pageNumber, pageSize);
                return Ok(projects);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "HRManager, Administrator, ProjectManager")]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                var project = await projectService.GetProjectByIdAsync(id);
                return Ok(project);
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
        [Authorize(Roles = "Administrator, ProjectManager")]
        public async Task<IActionResult> UpdateProject(int id, ProjectPostDTO project)
        {
            try
            {
                await projectService.UpdateProjectAsync(id, project);
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
        [Authorize(Roles = "Administrator, ProjectManager")]
        public async Task<IActionResult> DeactivateProject(int id)
        {
            try
            {
                await projectService.DeactivateProjectAsync(id);
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
