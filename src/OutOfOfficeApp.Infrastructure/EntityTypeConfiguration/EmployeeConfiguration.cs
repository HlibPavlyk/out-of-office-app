using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutOfOfficeApp.CoreDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOfficeApp.Infrastructure.EntityTypeConfiguration
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Subdivision)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(e => e.Position)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(e => e.Status)
                   .IsRequired()
                   .HasConversion<string>();

            builder.HasOne(e => e.PeoplePartner)
                   .WithMany()
                   .HasForeignKey(e => e.PeoplePartnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.OutOfOfficeBalance)
                   .IsRequired();
            
            builder.HasOne(e => e.Project)
                   .WithMany()
                   .IsRequired(false)
                   .HasForeignKey(e => e.ProjectId)
                   .OnDelete(DeleteBehavior.Restrict);
            

        }
    }
}
