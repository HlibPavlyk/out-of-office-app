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

namespace OutOfOfficeApp.Application.Services
{
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovalRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApprovalRequestGetDTO> GetApprovalRequestByIdAsync(int id)
        {
            var request = await _unitOfWork.ApprovalRequests.GetApprovalRequestWithDetailsAsync(id);
            if (request == null)
            {
                throw new ArgumentNullException("Approval requests not found");
            }

            var requestDTO = new ApprovalRequestGetDTO
            {
                Id = request.Id,
                Approver = new EmployeeNameDTO
                {
                    Id = request.Approver.Id,
                    FullName = request.Approver.FullName
                },
                LeaveRequest = new LeaveRequestGetDTO
                {
                    Id = request.LeaveRequest.Id,
                    Employee = new EmployeeNameDTO
                    {
                        Id = request.LeaveRequest.Employee.Id,
                        FullName = request.LeaveRequest.Employee.FullName
                    },
                    AbsenceReason = request.LeaveRequest.AbsenceReason,
                    StartDate = request.LeaveRequest.StartDate,
                    EndDate = request.LeaveRequest.EndDate,
                    Comment = request.LeaveRequest.Comment,
                    Status = request.LeaveRequest.Status
                },
                Status = request.Status,
                Comment = request.Comment
            };

            return requestDTO;
        }

        public async Task<PagedResponse<ApprovalRequestGetDTO>?> GetApprovalRequestsAsync(int pageNumber, int pageSize)
        {
            var requests = await _unitOfWork.ApprovalRequests.GetPagedApprovalRequestsWithDetailsAsync(pageNumber, pageSize);
            if (requests == null)
            {
                return null;
            }

            var requestDTOs = requests.Items.Select(ar => new ApprovalRequestGetDTO
            {
                Id = ar.Id,
                Approver = new EmployeeNameDTO
                {
                    Id = ar.Approver.Id,
                    FullName = ar.Approver.FullName
                },
                LeaveRequest = new LeaveRequestGetDTO
                {
                    Id = ar.LeaveRequest.Id,
                    Employee = new EmployeeNameDTO
                    {
                        Id = ar.LeaveRequest.Employee.Id,
                        FullName = ar.LeaveRequest.Employee.FullName
                    },
                    AbsenceReason = ar.LeaveRequest.AbsenceReason,
                    StartDate = ar.LeaveRequest.StartDate,
                    EndDate = ar.LeaveRequest.EndDate,
                    Comment = ar.LeaveRequest.Comment,
                    Status = ar.LeaveRequest.Status
                },
                Status = ar.Status,
                Comment = ar.Comment
            }).ToList();

            var response = new PagedResponse<ApprovalRequestGetDTO>
            {
                Items = requestDTOs,
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

        public async Task ApproveApprovalRequestAsync(int id, ApprovalRequestPostDTO issuerData)
        {
            await ChangeApprovalRequestStatusAsync(id, issuerData, true);
        }


        public async Task RejectApprovalRequestAsync(int id, ApprovalRequestPostDTO issuerData)
        {
            await ChangeApprovalRequestStatusAsync(id, issuerData, false);
        }

        private async Task<bool> ChangeApprovalRequestStatusAsync(int id,
            ApprovalRequestPostDTO issuerData, bool isApproved)
        {
            ApprovalRequestStatus approvalRequestStatus = ApprovalRequestStatus.Rejected;
            LeaveRequestStatus leaveRequestStatus = LeaveRequestStatus.Rejected;
            if (isApproved)
            {
                approvalRequestStatus = ApprovalRequestStatus.Approved;
                leaveRequestStatus = LeaveRequestStatus.Approved;
            }

            var request = await _unitOfWork.ApprovalRequests.GetByIdAsync(id);
            if (request == null)
            {
                throw new ArgumentNullException("Approval request not found");
            }
            if (request.Status != ApprovalRequestStatus.New)
            {
                throw new InvalidOperationException("Approval request is not in New status");
            }

            request.Status = approvalRequestStatus;
            request.Comment = issuerData.Comment;
            if(issuerData.IssuerId != null)
            {
                request.ApproverId = issuerData.IssuerId.Value;
            }

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

            return isApproved;
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
