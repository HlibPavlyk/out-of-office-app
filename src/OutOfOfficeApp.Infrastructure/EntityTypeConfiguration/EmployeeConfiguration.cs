using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutOfOfficeApp.Application.Entities;
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
                .IsRequired();

            builder.Property(e => e.Position)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired();

            builder.Property(e => e.PeoplePartner)
                .IsRequired();

            builder.Property(e => e.OutOfOfficeBalance)
                .IsRequired();

            builder.Property(e => e.Photo)
                .HasColumnType("varbinary(max)")
                .IsRequired(false);

            builder.HasOne(e => e.PeoplePartner)
                .WithMany();

        }
    }
}
