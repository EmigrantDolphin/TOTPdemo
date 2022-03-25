using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TOTPdemo.Domain;

namespace TOTPdemo.Persistence;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Email);
        builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Password).HasMaxLength(100).IsRequired();
        builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.TotpSecretCode).HasMaxLength(100).IsRequired(false);
    }
}