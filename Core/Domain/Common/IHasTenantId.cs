namespace Domain.Common;

public interface IHasTenantId
{
    string? TenantId { get; set; }
}

