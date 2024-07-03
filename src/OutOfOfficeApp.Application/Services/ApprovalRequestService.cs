using Azure.Core;
using Microsoft.EntityFrameworkCore;
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
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ApprovalRequestService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<ApprovalRequestGetDTO> GetApprovalRequestByIdAsync(int id)
        {
            var request = await _unitOfWork.ApprovalRequests.GetApprovalRequestWithDetailsAsync(id);
            if (request == null)
            {
                throw new ArgumentNullException("Approval requests not found");
            }

            var requestDto = new ApprovalRequestGetDTO
            {
                Id = request.Id,
                Approver = new EmployeeNameDTO
                {
                    Id = request.Approver.Id,
                    FullName = request.Approver.FullName
                },
                LeaveRequestId = request.LeaveRequestId,
                Status = request.Status,
                Comment = request.Comment
            };

            return requestDto;
        }

        public async Task<PagedResponse<ApprovalRequestGetDTO>?> GetApprovalRequestsAsync(string? userEmail,
            int pageNumber, int pageSize)
        {
            if (userEmail == null)                                                   
            {                                                                       
                throw new ArgumentNullException("User email not found");             
            }
            
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();
            
            var requests = await _unitOfWork.ApprovalRequests.GetPagedApprovalRequestsWithDetailsAsync(userRole, user.EmployeeId, pageNumber, pageSize);
            if (requests == null)
            {
                return null;
            }

            var requestDtos = requests.Items.Select(ar => new ApprovalRequestGetDTO
            {
                Id = ar.Id,
                Approver = new EmployeeNameDTO
                {
                    Id = ar.Approver.Id,
                    FullName = ar.Approver.FullName
                },
                LeaveRequestId = ar.LeaveRequestId, 
                Status = ar.Status,
                Comment = ar.Comment
            }).ToList();

            var response = new PagedResponse<ApprovalRequestGetDTO>
            {
                Items = requestDtos,
                TotalPages = requests.TotalPages
            };

            return response;
        }

        public async Task AddApprovalRequestAsync(int leaveRequestId)
        {
            var relatedApproverId = await _unitOfWork.Employees.GetHRManagerIdWithLeastActiveRequestsAsync();
            if (relatedApproverId == null)
            {
                throw new InvalidOperationException("No HR Manager found");
            }

            var newApprovalRequest = new ApprovalRequest
            {
                ApproverId = relatedApproverId.Value,
                LeaveRequestId = leaveRequestId,
                Status = ApprovalRequestStatus.New
            };

            await _unitOfWork.ApprovalRequests.AddAsync(newApprovalRequest);
            await _unitOfWork.CompleteAsync();
        }

        public async Task ApproveApprovalRequestAsync(int id, string? userEmail, ApprovalRequestPostDTO issuerData)
        {
            await ChangeApprovalRequestStatusAsync(id, userEmail, issuerData, true);
        }


        public async Task RejectApprovalRequestAsync(int id, string? userEmail, ApprovalRequestPostDTO issuerData)
        {
            await ChangeApprovalRequestStatusAsync(id, userEmail, issuerData, false);
        }

        private async Task ChangeApprovalRequestStatusAsync(int id,
            string? userEmail, ApprovalRequestPostDTO issuerData, bool isApproved)
        {
            ApprovalRequestStatus approvalRequestStatus = ApprovalRequestStatus.Rejected;
            LeaveRequestStatus leaveRequestStatus = LeaveRequestStatus.Rejected;
            if (isApproved)
            {
                approvalRequestStatus = ApprovalRequestStatus.Approved;
                leaveRequestStatus = LeaveRequestStatus.Approved;
            }
            
            if (userEmail == null)                                                   
            {                                                                       
                throw new ArgumentNullException("User email not found");             
            }
            
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();
            

            var request = await _unitOfWork.ApprovalRequests.GetApprovalRequestWithDetailsAsync(id);
            if (request == null)
            {
                throw new ArgumentNullException("Approval request not found");
            }
            if (request.Status != ApprovalRequestStatus.New)
            {
                throw new InvalidOperationException("Approval request is not in New status");
            }
            
            if (request.ApproverId != user.EmployeeId && userRole != "Administrator"
                && request.LeaveRequest.Employee.Project.ProjectManagerId != user.EmployeeId)
            {
                throw new InvalidOperationException("User is not authorized to approve/reject this request");
            }

            request.Status = approvalRequestStatus;
            request.Comment = issuerData.Comment;
            request.ApproverId = user.EmployeeId;

            var leaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(request.LeaveRequestId);
            if (leaveRequest == null)
            {
                throw new ArgumentNullException("Leave request not found");
            }
            if (leaveRequest.Status != LeaveRequestStatus.Submitted)
            {
                throw new InvalidOperationException("Leave request is not in Submitted status");
            }

            leaveRequest.Status = leaveRequestStatus;
            if(isApproved)
            {
                await CalculateOutOfOfficeBalanceChangeAsync(leaveRequest.StartDate,
                    leaveRequest.EndDate, leaveRequest.EmployeeId);
            }

            _unitOfWork.ApprovalRequests.Update(request);
            _unitOfWork.LeaveRequests.Update(leaveRequest);
            await _unitOfWork.CompleteAsync();
        }
        
        private async Task CalculateOutOfOfficeBalanceChangeAsync(DateOnly startDate, DateOnly endDate, int employeeId)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new InvalidOperationException("Employee not found");
            }

            int totalDays = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days + 1;

            if (totalDays > employee.OutOfOfficeBalance)
            {
                throw new InvalidOperationException("Invalid number of days");
            }

            employee.OutOfOfficeBalance -= totalDays;

            _unitOfWork.Employees.Update(employee);
            await _unitOfWork.CompleteAsync();
        }
    }
}
