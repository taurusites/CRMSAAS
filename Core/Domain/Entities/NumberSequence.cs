using Domain.Common;

namespace Domain.Entities;

public class NumberSequence : BaseEntity, IHasTenantId
{
    public string? TenantId { get; set; }
    public string? EntityName { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public int? LastUsedCount { get; set; }
}

