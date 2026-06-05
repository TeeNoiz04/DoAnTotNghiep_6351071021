using System;

namespace QuoteFlow.DPOs;
public class DpoGicAllocationDto
{
    public Guid Id { get; set; }
    public string? MaterialType { get; set; }
    public string DPONo { get; set; } = null!;
    public Guid? BuyerId { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public string? BuyerTypeDescription { get; set; }
    public string? BuyerShortName { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Remark { get; set; }
    public string? LinkedNote { get; set; }
}
