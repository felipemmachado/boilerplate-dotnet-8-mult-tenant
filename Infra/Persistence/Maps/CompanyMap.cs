using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infra.Persistence.Maps
{
    public class CompanyMap : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable(nameof(Company));

            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id);

            builder
                .Property(p => p.Url)
                .IsRequired();

            builder
                .OwnsOne(p => p.Logo, l =>
                {
                    l.Property(p => p.ResourceName).IsRequired();
                    l.Property(p => p.ResourceKey).IsRequired();
                });
            
            builder
                .OwnsOne(p => p.Customization, c =>
                {
                    c.Property(p => p.PrimaryColor).IsRequired();
                    c.Property(p => p.ButtonColor).IsRequired();
                    c.Property(p => p.TabBrowserTitle).IsRequired();
                });

            builder
                .Property(p => p.CompanyName)
                .IsRequired();

            builder
                .Property(p => p.TradingName)
                .IsRequired();

            builder
                .Property(p => p.Document)
                .IsRequired();

            builder
                .HasMany(p => p.Users)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.TenantId);

            builder
                .HasIndex(p => p.Url).IsUnique();

            builder
                .HasIndex(p => p.Document).IsUnique();
        }
    }
}
