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
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProjectType)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(p => p.StartDate)
                   .IsRequired();

            builder.Property(p => p.EndDate)
                   .IsRequired(false);

            builder.HasOne(p => p.ProjectManager)
                   .WithMany()
                   .HasForeignKey(p => p.ProjectManagerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasDefaultValue(ActiveStatus.Active);

            builder.Property(p => p.Comment)
                   .IsRequired(false)
                   .HasMaxLength(1000);
        }
    }
}
