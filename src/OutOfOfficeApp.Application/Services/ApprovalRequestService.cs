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

        public Task<ApprovalRequestGetDTO> GetApprovalRequestByIdAsync(int id)
        {
            throw new NotImplementedException();
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

        public async Task AddApprovalRequestAsync(ApprovalRequestPostDTO request)
        {
            var relatedApproverId = await _unitOfWork.Employees.GetHRManagerIdWithLeastActiveRequestsAsync();
            if (relatedApproverId == null)
            {
                throw new InvalidOperationException("No HR Manager found");
            }

            var newApprovalRequest = new ApprovalRequest
            {
                ApproverId = relatedApproverId.Value,
                LeaveRequestId = request.LeaveRequestId,
                Status = ApprovalRequestStatus.New,
                Comment = request.Comment
            };

            await _unitOfWork.ApprovalRequests.AddAsync(newApprovalRequest);
            await _unitOfWork.CompleteAsync();
        }

        public Task ApproveApprovalRequestAsync(int id, int issuerId)
        {
            throw new NotImplementedException();
        }

        public Task RejectApprovalRequestAsync(int id, int issuerId)
        {
            throw new NotImplementedException();
        }
    }
}
