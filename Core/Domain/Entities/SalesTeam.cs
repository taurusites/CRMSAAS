using Domain.Common;

namespace Domain.Entities;

public class SalesTeam : BaseEntity, IHasTenantId
{
    public string? TenantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<SalesRepresentative> SalesRepresentativeList { get; set; } = new List<SalesRepresentative>();
}
