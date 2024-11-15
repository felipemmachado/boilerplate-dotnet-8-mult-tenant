using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infra.Persistence.Factories;
using Infra.Persistence.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Reflection;
namespace Infra.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IUserService _userService;
        public readonly Guid? CurrentTenantId;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IUserService userService,
            ICurrentTenantService currentTenantService,
            IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
            CurrentTenantId = currentTenantService.TenantId;
            _userService = userService;
        }

        protected IConfiguration Configuration { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            AddFieldControl();

            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.FilterTenancy(CurrentTenantId);

            base.OnModelCreating(builder);
        }

        private void AddFieldControl()
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _userService.UserId == Guid.Empty ? null : _userService.UserId;
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _userService.UserId == Guid.Empty ? null : _userService.UserId;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Detached:
                        break;

                    case EntityState.Unchanged:
                        break;

                    case EntityState.Deleted:
                        break;
                }
            }

            var tenancy = CurrentTenantId;

            if (tenancy == null)
                return;

            var tenancyEntities = ChangeTracker
                .Entries()
                .Where(c => c.State == EntityState.Added)
                .Select(c => c.Entity).OfType<TenancyBase>();

            foreach (var entity in tenancyEntities)
            {
                entity.TenantId = tenancy.Value;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(Configuration.GetConnectionString("AppConnection"))
                .ReplaceService<IModelCacheKeyFactory, ContextModelCacheKeyFactory>();
        }
    }
}
