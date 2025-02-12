using Domain.Common;

namespace Domain.Entities;

public class BookingResource : BaseEntity, IHasTenantId
{
    public string? TenantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? BookingGroupId { get; set; }
    public BookingGroup? BookingGroup { get; set; }
}
