using Microsoft.EntityFrameworkCore;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.Infrastructure.DTO;
using OutOfOfficeApp.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Project?> GetProjectWithDetailsAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.ProjectManager)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResponse<Project>?> GetPagedProjectsWithDetailsAsync(int pageNumber, int pageSize)
        {
            var items = await _context.Projects
               .Include(p => p.ProjectManager)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            var totalItems = await _context.Projects.CountAsync();
            if (items == null || totalItems == 0)
            {
                return null;
            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return new PagedResponse<Project>
            {
                Items = items,
                TotalPages = totalPages
            };
        }
    }
}
