using Domain.Common;

namespace Domain.Entities;

public class Warehouse : BaseEntity, IHasTenantId
{
    public string? TenantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? SystemWarehouse { get; set; } = false;
}
