using Domain.Common;

namespace Domain.Entities;

public class TenantMember : BaseEntity
{

    public string? UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TenantId { get; set; }
    public Tenant? Tenant { get; set; }



}
