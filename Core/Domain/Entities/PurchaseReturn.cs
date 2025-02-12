using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class PurchaseReturn : BaseEntity, IHasTenantId
{
    public string? TenantId { get; set; }
    public string? Number { get; set; }
    public DateTime? ReturnDate { get; set; }
    public PurchaseReturnStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? GoodsReceiveId { get; set; }
    public GoodsReceive? GoodsReceive { get; set; }
}
