using Application.Common.Interfaces;
namespace Infra.Services
{
    public class CurrentTenantService : ICurrentTenantService
    {
        public bool SetTenant(Guid? tenantId)
        {
            TenantId = tenantId;
            return true;
        }
        public Guid? TenantId { get; set; }
    }
}
