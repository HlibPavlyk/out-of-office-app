using Azure.Core;
using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using OutOfOfficeApp.Infrastructure.DTO;
using OutOfOfficeApp.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectGetDTO> GetProjectByIdAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetProjectWithDetailsAsync(id);
            if (project == null)
            {
                throw new ArgumentNullException("Project not found");
            }

            var projectDTO = new ProjectGetDTO
            {
                Id = project.Id,
                ProjectType = project.ProjectType,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ProjectManager = new EmployeeNameDTO
                {
                    Id = project.ProjectManager.Id,
                    FullName = project.ProjectManager.FullName
                },
                Comment = project.Comment,
                Status = project.Status
            };

            return projectDTO;
        }

        public async Task<PagedResponse<ProjectGetDTO>?> GetProjectsAsync(int pageNumber, int pageSize)
        {
            var projects = await _unitOfWork.Projects.GetPagedProjectsWithDetailsAsync(pageNumber, pageSize);
            if (projects == null)
            {
                return null;
            }

            var projectDTOs = projects.Items.Select(e => new ProjectGetDTO
            {
                Id = e.Id,
                ProjectType = e.ProjectType,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                ProjectManager = new EmployeeNameDTO
                {
                    Id = e.ProjectManager.Id,
                    FullName = e.ProjectManager.FullName
                },
                Comment = e.Comment,
                Status = e.Status
            }).ToList();

            var response = new PagedResponse<ProjectGetDTO>
            {
                Items = projectDTOs,
                TotalPages = projects.TotalPages
            };

            return response;
        }

        public async Task AddProjectAsync(ProjectPostDTO project)
        {
            if (project.EndDate != null && project.StartDate > project.EndDate)
            {
                throw new InvalidOperationException("Start date cannot be after end date");
            }

            var projectManager = await _unitOfWork.Employees.GetByIdAsync(project.ProjectManagerId);
            if (projectManager == null)
            {
                throw new InvalidOperationException("Employee with that ProjectManagerId not found");
            }
            if (projectManager.Status == ActiveStatus.Inactive ||projectManager.Position != Position.ProjectManager)
            {
                throw new InvalidOperationException("Employee is not Active or is not a Project Manager");
            }

            var newProject = new Project
            {
                ProjectType = project.ProjectType,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ProjectManagerId = project.ProjectManagerId,
                Comment = project.Comment,
                Status = project.Status
            };

            await _unitOfWork.Projects.AddAsync(newProject);
            await _unitOfWork.CompleteAsync();
        }
        public async Task UpdateProjectAsync(int id, ProjectPostDTO project)
        {
            var existingProject = await _unitOfWork.Projects.GetByIdAsync(id);

            if (existingProject == null)
            {
                throw new ArgumentNullException("Project not found");
            }
            else
            {
                existingProject.ProjectType = project.ProjectType;
                existingProject.StartDate = project.StartDate;
                existingProject.EndDate = project.EndDate;
                existingProject.ProjectManagerId = project.ProjectManagerId;
                existingProject.Comment = project.Comment;
                existingProject.Status = project.Status;

                _unitOfWork.Projects.Update(existingProject);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeactivateProjectAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);

            if (project == null)
            {
                throw new ArgumentNullException("Project not found");
            }
            if(project.Status == ActiveStatus.Inactive)
            {
                throw new InvalidOperationException("Project is already inactive");
            }

            project.Status = ActiveStatus.Inactive;
            _unitOfWork.Projects.Update(project);
            await _unitOfWork.CompleteAsync();

        }
    }
}
