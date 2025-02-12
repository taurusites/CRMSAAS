using Domain.Common;

namespace Domain.Entities;

public class Tax : BaseEntity, IHasTenantId
{
    public string? TenantId { get; set; }
    public string? Name { get; set; }
    public double? Percentage { get; set; }
    public string? Description { get; set; }
}
