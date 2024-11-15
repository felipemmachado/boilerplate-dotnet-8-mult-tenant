using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infra.Persistence.Maps
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id);

            builder.Property(p => p.Roles).HasColumnType("text[]");

            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.ForceChangePassword);

            builder.HasIndex(p => new
            {
                p.Email,
                p.TenantId
            }).IsUnique();
        }
    }
}
