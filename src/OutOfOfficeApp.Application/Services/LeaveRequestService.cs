using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public LeaveRequestService(IUnitOfWork unitOfWork, IApprovalRequestService approvalRequestService,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _approvalRequestService = approvalRequestService;
            _userManager = userManager;
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

        public async Task<PagedResponse<LeaveRequestGetDTO>?> GetLeaveRequestsAsync(string? userEmail, int pageNumber, int pageSize)
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

            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();
            
            var requests = await _unitOfWork.LeaveRequests.GetPagedLeaveRequestsWithDetailsAsync(userRole,
                user.EmployeeId, pageNumber, pageSize);
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

        public async  Task AddLeaveRequestAsync(string? userEmail, LeaveRequestPostDTO request)
        {
            if (userEmail == null)                                                   
            {                                                                       
                throw new ArgumentNullException("User email not found");             
            }
            if(request.StartDate > request.EndDate)
            {
                throw new InvalidOperationException("Start date cannot be after end date");
            }
            
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)                                                   
            {                                                                       
                throw new ArgumentNullException("User not found");             
            } 
        
            var newLeaveRequest = new LeaveRequest
            {
                EmployeeId = user.EmployeeId,
                AbsenceReason = request.AbsenceReason,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Comment = request.Comment,
            };

            await _unitOfWork.LeaveRequests.AddAsync(newLeaveRequest);
            await _unitOfWork.CompleteAsync();
        }
        public async  Task UpdateLeaveRequestAsync(int id, string? userEmail, LeaveRequestPostDTO request)
        {
            if (userEmail == null)                                                   
            {                                                                       
                throw new ArgumentNullException("User email not found");             
            }
            if(request.StartDate > request.EndDate)
            {
                throw new InvalidOperationException("Start date cannot be after end date");
            }
            
            var existingLeaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(id);
            if (existingLeaveRequest == null)
            {
                throw new ArgumentNullException("Leave requests not found");
            }
            
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();
            if (user.EmployeeId != existingLeaveRequest.EmployeeId && userRole != Position.Administrator.ToString())
            {
                throw new InvalidOperationException("That is not current employee request");
            }

            if (existingLeaveRequest.Status != LeaveRequestStatus.New)
            {
                throw new InvalidOperationException("Cannot update leave request that is not in New status");
            }
            else
            {
                existingLeaveRequest.AbsenceReason = request.AbsenceReason;
                existingLeaveRequest.StartDate = request.StartDate;
                existingLeaveRequest.EndDate = request.EndDate;
                existingLeaveRequest.Comment = request.Comment;

                _unitOfWork.LeaveRequests.Update(existingLeaveRequest);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task SubmitLeaveRequestAsync(string? userEmail, int id)
        {
            if (userEmail == null)                                                   
            {                                                                       
                throw new ArgumentNullException("User email not found");             
            }
            
            var existingLeaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(id);
            if (existingLeaveRequest == null)
            {
                throw new ArgumentNullException("Leave requests not found");
            }
            
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();
            if (user.EmployeeId != existingLeaveRequest.EmployeeId && userRole != Position.Administrator.ToString())
            {
                throw new InvalidOperationException("That is not current employee request");
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
        public async Task CancelLeaveRequestAsync(string? userEmail, int id)
        {
            if (userEmail == null)                                                   
            {                                                                       
                throw new ArgumentNullException("User email not found");             
            }
            
            var existingLeaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(id);
            if (existingLeaveRequest == null)
            {
                throw new ArgumentNullException("Leave requests not found");
            }
            
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();
            if (user.EmployeeId != existingLeaveRequest.EmployeeId && userRole != Position.Administrator.ToString())
            {
                throw new InvalidOperationException("That is not current employee request");
            }

            if (existingLeaveRequest.Status == LeaveRequestStatus.Canceled)
            {
                throw new InvalidOperationException("Cannot cancel leave request that is already canceled");
            }
           
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

            if (existingLeaveRequest.Status == LeaveRequestStatus.Approved)
            {
                throw new InvalidOperationException("Cannot cancel approved leave request"); 
            }

            existingLeaveRequest.Status = LeaveRequestStatus.Canceled;

            _unitOfWork.LeaveRequests.Update(existingLeaveRequest);
            await _unitOfWork.CompleteAsync();
        }
        
    }
}
