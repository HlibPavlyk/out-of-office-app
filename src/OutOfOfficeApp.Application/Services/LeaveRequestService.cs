using OutOfOfficeApp.Application.DTO;
using OutOfOfficeApp.Application.Services.Interfaces;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using OutOfOfficeApp.Infrastructure.DTO;
using OutOfOfficeApp.Infrastructure.Repositories.Interfaces;

namespace OutOfOfficeApp.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApprovalRequestService _approvalRequestService;

        public LeaveRequestService(IUnitOfWork unitOfWork, IApprovalRequestService approvalRequestService)
        {
            _unitOfWork = unitOfWork;
            _approvalRequestService = approvalRequestService;
        }

        public async Task<LeaveRequestGetDTO> GetLeaveRequestByIdAsync(int id)
        {
            var request = await _unitOfWork.LeaveRequests.GetLeaveRequestWithDetailsAsync(id);
            if (request == null)
            {
                throw new ArgumentNullException("Leave requests not found");
            }

            var requestDTO = new LeaveRequestGetDTO
            {
                Id = request.Id,
                Employee = new EmployeeNameDTO
                {
                    Id = request.Employee.Id,
                    FullName = request.Employee.FullName
                },
                AbsenceReason = request.AbsenceReason,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Comment = request.Comment,
                Status = request.Status
            };

            return requestDTO;
        }

        public async Task<PagedResponse<LeaveRequestGetDTO>?> GetLeaveRequestsAsync(int pageNumber, int pageSize)
        {
            var requests = await _unitOfWork.LeaveRequests.GetPagedLeaveRequestsWithDetailsAsync(pageNumber, pageSize);
            if (requests == null)
            {
                return null;
            }

            var requestDTOs = requests.Items.Select(lr => new LeaveRequestGetDTO
            {
                Id = lr.Id,
                Employee = new EmployeeNameDTO
                {
                    Id = lr.Employee.Id,
                    FullName = lr.Employee.FullName
                },
                AbsenceReason = lr.AbsenceReason,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                Comment = lr.Comment,
                Status = lr.Status
            }).ToList();

            var response = new PagedResponse<LeaveRequestGetDTO>
            {
                Items = requestDTOs,
                TotalPages = requests.TotalPages
            };

            return response;
        }

        public async  Task AddLeaveRequestAsync(LeaveRequestPostDTO request)
        {
            if(request.StartDate > request.EndDate)
            {
                throw new InvalidOperationException("Start date cannot be after end date");
            }

            var issuer = await _unitOfWork.Employees.GetEmployeeWithDetailsAsync(request.EmployeeId);
            if (issuer == null)
            {
                throw new ArgumentNullException("Employee with that id not found");
            }

            var newLeaveRequest = new LeaveRequest
            {
                EmployeeId = request.EmployeeId,
                AbsenceReason = request.AbsenceReason,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Comment = request.Comment,
            };

            await _unitOfWork.LeaveRequests.AddAsync(newLeaveRequest);
            await _unitOfWork.CompleteAsync();
        }
        public async  Task UpdateLeaveRequestAsync(int id, LeaveRequestPostDTO employee)
        {
            var existingLeaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(id);

            if (existingLeaveRequest == null)
            {
                throw new ArgumentNullException("Leave requests not found");
            }

            if (existingLeaveRequest.Status != LeaveRequestStatus.New)
            {
                throw new InvalidOperationException("Cannot update leave request that is not in New status");
            }
            else
            {
                existingLeaveRequest.EmployeeId = employee.EmployeeId;
                existingLeaveRequest.AbsenceReason = employee.AbsenceReason;
                existingLeaveRequest.StartDate = employee.StartDate;
                existingLeaveRequest.EndDate = employee.EndDate;
                existingLeaveRequest.Comment = employee.Comment;

                _unitOfWork.LeaveRequests.Update(existingLeaveRequest);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task SubmitLeaveRequestAsync(int id)
        {
            var existingLeaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(id);

            if (existingLeaveRequest == null)
            {
                throw new ArgumentNullException("Leave requests not found");
            }

            if (existingLeaveRequest.Status != LeaveRequestStatus.New)
            {
                throw new InvalidOperationException("Cannot submit leave request that is not in New status");
            }
            else
            {
                existingLeaveRequest.Status = LeaveRequestStatus.Submitted;

                _unitOfWork.LeaveRequests.Update(existingLeaveRequest);
                await _unitOfWork.CompleteAsync();
            }

            await _approvalRequestService.AddApprovalRequestAsync(id);
        }
        public async Task CancelLeaveRequestAsync(int id)
        {
            var existingLeaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(id);

            if (existingLeaveRequest == null)
            {
                throw new ArgumentNullException("Leave requests not found");
            }

            if (existingLeaveRequest.Status == LeaveRequestStatus.Canceled)
            {
                throw new InvalidOperationException("Cannot cancel leave request that is already canceled");
            }
            else
            {
                if (existingLeaveRequest.Status == LeaveRequestStatus.Submitted)
                {
                    var existingApprovalRequest = await _unitOfWork.ApprovalRequests
                        .GetApprovalRequestByLeaveRequestIdAsync(id);

                    if (existingApprovalRequest == null)
                    {
                        throw new ArgumentNullException("Approval request not found");
                    }

                    existingApprovalRequest.Status = ApprovalRequestStatus.Rejected;

                    _unitOfWork.ApprovalRequests.Update(existingApprovalRequest);
                    await _unitOfWork.CompleteAsync();
                }

                existingLeaveRequest.Status = LeaveRequestStatus.Canceled;

                _unitOfWork.LeaveRequests.Update(existingLeaveRequest);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
