using Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Application.UnitTests.Mocks
{
    public class TestContextModalCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context, bool designTime = false)
        {
            return context is not ApplicationDbContext dynamicContext
                ? context.GetType()
                : (context.GetType(), dynamicContext.CurrentTenantId, designTime);
        }

    }
}
