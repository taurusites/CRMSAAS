using Domain.Common;

namespace Domain.Entities;

public class Tenant : BaseEntity
{
    public string? Name { get; set; }
    public string? Reference { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public ICollection<TenantMember> TenantMemberList { get; set; } = new List<TenantMember>();


}
