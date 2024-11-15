using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Infra.Persistence.Factories
{
    public class ContextModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context, bool designTime = false)
        {
            return context is not ApplicationDbContext dynamicContext
                ? context.GetType()
                : (context.GetType(), dynamicContext.CurrentTenantId, designTime);
        }

    }
}
