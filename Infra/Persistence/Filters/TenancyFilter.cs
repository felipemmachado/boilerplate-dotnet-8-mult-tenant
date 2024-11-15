using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infra.Persistence.Filters
{
    public static class TenancyFilter
    {
        public static ModelBuilder FilterTenancy(this ModelBuilder modelBuilder, Guid? tenantId)
        {
            modelBuilder.Entity<User>()
                .HasQueryFilter(x => x.TenantId == tenantId);

            return modelBuilder;
        }
    }
}
