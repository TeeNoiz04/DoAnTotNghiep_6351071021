using System;

namespace QuoteFlow.DPOs;

public class DPOExcelDto
{
    public string DPONo { get; set; } = null!;
    public string? DPOType { get; set; }
    public string? DPOSubType { get; set; }
    public string? MaterialType { get; set; }
    public string? CostCenter { get; set; }
    public string? Status { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerShortName { get; set; }
    public string? BuyerDescription { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Remark { get; set; }
    public string? FileName { get; set; }
}