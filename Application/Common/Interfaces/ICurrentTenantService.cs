namespace Application.Common.Interfaces
{
    public interface ICurrentTenantService
    {
        public Guid? TenantId { get; set; }

        public bool SetTenant(Guid? tenantId);
    }
}
