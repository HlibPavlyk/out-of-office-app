using Microsoft.AspNetCore.Mvc;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;

namespace OutOfOfficeApp.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectPostDTO project)
        {
            try
            {
                await _projectService.AddProjectAsync(project);
                return Created();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var projects = await _projectService.GetProjectsAsync(pageNumber, pageSize);
                return Ok(projects);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                var project = await _projectService.GetProjectByIdAsync(id);
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
        public async Task<IActionResult> UpdateProject(int id, ProjectPostDTO project)
        {
            try
            {
                await _projectService.UpdateProjectAsync(id, project);
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
        public async Task<IActionResult> DeactivateProject(int id)
        {
            try
            {
                await _projectService.DeactivateProjectAsync(id);
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
