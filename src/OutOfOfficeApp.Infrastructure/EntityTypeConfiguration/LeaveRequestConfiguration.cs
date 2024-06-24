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
    internal class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.HasKey(lr => lr.Id);

            builder.HasOne(lr => lr.Employee)
                   .WithMany()
                   .HasForeignKey(lr => lr.EmployeeId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(lr => lr.AbsenceReason)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(lr => lr.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasDefaultValue(LeaveRequestStatus.New);

            builder.Property(lr => lr.StartDate)
                   .IsRequired();

            builder.Property(lr => lr.EndDate)
                   .IsRequired();

            builder.Property(lr => lr.Comment)
                   .IsRequired(false)
                   .HasMaxLength(1000);
        }
    }
}
