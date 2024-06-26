using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.Services.Interfaces
{
    public interface IProjectService
    {
        Task<PagedResponse<ProjectGetDTO>?> GetProjectsAsync(int pageNumber, int pageSize);
        Task<ProjectGetDTO> GetProjectByIdAsync(int id);
        Task AddProjectAsync(ProjectPostDTO project);
        Task UpdateProjectAsync(int id, ProjectPostDTO project);
        Task DeactivateProjectAsync(int id);
    }
}
