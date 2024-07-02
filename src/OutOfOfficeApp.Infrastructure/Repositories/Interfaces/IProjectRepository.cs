using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.Repositories.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<Project?> GetProjectWithDetailsAsync(int id);
        Task<PagedResponse<Project>?> GetPagedProjectsWithDetailsAsync(int pageNumber, int pageSize,
            int? isHrManagerRequest = null);
    }
}
