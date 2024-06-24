using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutOfOfficeApp.CoreDomain.Entities;
using OutOfOfficeApp.CoreDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.EntityTypeConfiguration
{
    internal class ApprovalRequestConfiguration : IEntityTypeConfiguration<ApprovalRequest>
    {
        public void Configure(EntityTypeBuilder<ApprovalRequest> builder)
        {
            builder.HasKey(ar => ar.Id);

            builder.HasOne(ar => ar.Approver)
                   .WithMany()
                   .HasForeignKey(ar => ar.ApproverId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ar => ar.LeaveRequest)
                   .WithMany()
                   .HasForeignKey(ar => ar.LeaveRequestId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ar => ar.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasDefaultValue(ApprovalRequestStatus.New);

            builder.Property(ar => ar.Comment)
                   .IsRequired(false)
                   .HasMaxLength(1000);
        }
    }
}
