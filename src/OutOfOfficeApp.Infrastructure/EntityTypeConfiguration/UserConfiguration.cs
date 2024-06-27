using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutOfOfficeApp.CoreDomain.Entities;

namespace OutOfOfficeApp.Infrastructure.EntityTypeConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(e => e.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
