using Application.Common.Interfaces;
using Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
namespace Application.UnitTests.Mocks
{
    public class TestApplicationDbContext: ApplicationDbContext
    {
        public TestApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options, 
            IUserService userService, 
            ICurrentTenantService currentTenantService,
            IConfiguration configuration) : base(options, userService, currentTenantService, configuration)
        {
            Configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Configuration.GetSection("ConnectionStrings:AppConnection").Value
                ?? throw new Exception("Connection string not found");
            
            optionsBuilder
                .UseInMemoryDatabase(connectionString)
                .ReplaceService<IModelCacheKeyFactory, TestContextModalCacheKeyFactory>();
        }
    }
}
