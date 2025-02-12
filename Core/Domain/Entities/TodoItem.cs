using Domain.Common;

namespace Domain.Entities;

public class TodoItem : BaseEntity, IHasTenantId
{
    public string? TenantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TodoId { get; set; }
    public Todo? Todo { get; set; }

}
