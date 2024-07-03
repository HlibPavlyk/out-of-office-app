using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace OutOfOfficeApp.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository Employees { get; }
        ILeaveRequestRepository LeaveRequests { get; }
        IApprovalRequestRepository ApprovalRequests { get; }
        IProjectRepository Projects { get; }
        Task CompleteAsync();
       // Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
