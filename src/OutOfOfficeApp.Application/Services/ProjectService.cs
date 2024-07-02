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
using Microsoft.AspNetCore.Identity;

namespace OutOfOfficeApp.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ProjectService(IUnitOfWork unitOfWork,  UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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

        public async Task<PagedResponse<ProjectGetDTO>?> GetProjectsAsync(string? userEmail, int pageNumber, int pageSize)
        {
            if (userEmail == null)                                                   
            {                                                                       
                throw new ArgumentNullException("User email not found");             
            }  
            
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)                                                   
            {                                                                       
                throw new ArgumentNullException("User not found");             
            } 
            
            var userRole = await _userManager.GetRolesAsync(user);
            int? hrManagerId = null;
            if (userRole.FirstOrDefault() == Position.HRManager.ToString())
            {
                hrManagerId = user.EmployeeId;
            }
            
            var projects = await _unitOfWork.Projects.GetPagedProjectsWithDetailsAsync(pageNumber, pageSize, hrManagerId);
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
            var projectManager = await _unitOfWork.Employees.GetByIdAsync(project.ProjectManagerId);
            if (projectManager == null)
            {
                throw new InvalidOperationException("Employee with that ProjectManagerId not found");
            }
            if (projectManager.Status == ActiveStatus.Inactive ||projectManager.Position != Position.ProjectManager)
            {
                throw new InvalidOperationException("Employee is not Active or is not a Project Manager");
            }
            
            var today = DateOnly.FromDateTime(DateTime.Today);
            var isCompleted = project.Status == ActiveStatus.Inactive;

            var newProject = new Project
            {
                ProjectType = project.ProjectType,
                StartDate = today,
                EndDate = isCompleted ? today : null,
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

            if (existingProject.Status != project.Status)
            {
                var isCompleted = project.Status == ActiveStatus.Inactive;
                existingProject.EndDate = isCompleted ? DateOnly.FromDateTime(DateTime.Today) : null;
            }
            
            existingProject.ProjectType = project.ProjectType;
            existingProject.ProjectManagerId = project.ProjectManagerId;
            existingProject.Comment = project.Comment;
            existingProject.Status = project.Status;

            _unitOfWork.Projects.Update(existingProject);
            await _unitOfWork.CompleteAsync();
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
            project.EndDate = DateOnly.FromDateTime(DateTime.Today);
            
            _unitOfWork.Projects.Update(project);
            await _unitOfWork.CompleteAsync();

        }
    }
}
