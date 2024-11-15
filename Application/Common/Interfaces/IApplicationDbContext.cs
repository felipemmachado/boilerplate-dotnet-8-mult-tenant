using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
