using Domain.Common;

namespace Domain.Entities;

public class Todo : BaseEntity, IHasTenantId
{
    public string? TenantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<TodoItem> TodoItemList { get; set; } = new List<TodoItem>();
}
