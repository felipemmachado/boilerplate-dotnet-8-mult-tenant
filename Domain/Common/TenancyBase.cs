using Domain.Entities;
namespace Domain.Common
{
    public abstract class TenancyBase : EntityBase
    {
        public Guid TenantId { get; set; }
        public Company? Company { get; protected set; }
    }
}
