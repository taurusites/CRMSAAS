using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Invoice : BaseEntity, IHasTenantId
{
    public string? TenantId { get; set; }
    public string? Number { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public InvoiceStatus? InvoiceStatus { get; set; }
    public string? Description { get; set; }
    public string? SalesOrderId { get; set; }
    public SalesOrder? SalesOrder { get; set; }
}
